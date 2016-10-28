namespace Dixie.Microsoft
{
    using Dixie.Microsoft.Abstractions;
    using Dixie.Microsoft.Attribute;
    using Dixie.Microsoft.Internal;
    using Dixie.Microsoft.Util;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.DependencyInjection.Extensions;
    using global::Microsoft.Extensions.DependencyModel;
    using global::Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class ScanService : IScanService
    {
        private readonly IServiceProvider provider;
        private readonly ILogger log;

        private readonly Dictionary<Type, List<ServiceDescriptor>> scanned = new Dictionary<Type, List<ServiceDescriptor>>();

        /// <summary>
        /// Only custom assemblies allowed. list below conditions will be filtered.
        /// mscorlib, System, all assemblies from System or Microsoft.
        /// </summary>
        private readonly Func<RuntimeLibrary, bool> InternalFilter =
            library =>
                library.Name.Equals("mscorlib") || library.Name.Equals("System")
                || library.Name.StartsWith("System.") || library.Name.StartsWith("Microsoft.");

        public ScanService(IServiceCollection collection, ILoggerFactory logger, IServiceProvider provider, Type[] types)
        {
            this.log = logger.CreateLogger(nameof(IScanService));
            this.provider = provider;
            this.Collector(types, collection);
        }

        public ScanService(IServiceCollection collection, ILoggerFactory logger, IServiceProvider provider, Assembly[] asms)
        {
            this.log = logger.CreateLogger(nameof(IScanService));
            Dictionary<Type[], int> types = new Dictionary<Type[], int>();
            int idx = 0;
            foreach (var asm in asms)
            {
                var ts = asm.GetExportedTypes();
                types.Add(ts, ts.Length);
                idx += ts.Length;
            }

            var result = new Type[idx];
            idx = 0;
            foreach (var type in types)
            {
                type.Key.CopyTo(result, idx);
                idx += type.Value;
            }

            this.provider = provider;
            this.Collector(result, collection);
        }

        public ScanService(IServiceCollection collection, IServiceProvider provider, DependencyContext context)
        {
            Dictionary<Type[], int> types = new Dictionary<Type[], int>();
            int idx = 0;
            foreach (var library in context.RuntimeLibraries)
            {
                if (this.InternalFilter.Invoke(library)) continue;
                var ts = Assembly.Load(new AssemblyName(library.Name)).GetExportedTypes();
                types.Add(ts, ts.Length);
                idx += ts.Length;
            }

            var result = new Type[idx];
            idx = 0;
            foreach (var type in types)
            {
                type.Key.CopyTo(result, idx);
                idx += type.Value;
            }

            this.provider = provider;
            this.Collector(result, collection);
        }

        private void Collector(Type[] types, IServiceCollection collection)
        {
            Dictionary<Type, ScanDescriptor> collect = new Dictionary<Type, ScanDescriptor>(types.Length);
            foreach (var type in types)
            {
                var info = type.GetTypeInfo();
                if (!info.IsClass)
                {
                    this.log.LogDebug($"{type.FullName} is not a class. skipping.");
                    continue;
                }
                var attr = info.GetCustomAttribute<ServiceAttribute>();
                if (attr == null) continue;
                this.log.LogDebug($"Scanning service Type: {type.FullName}");
                if (info.IsGenericType && info.IsGenericTypeDefinition)
                {
                    this.log.LogDebug($"{type.FullName} have no generic arguments and have a just defination. cannot add to service. skipping.");
                    continue;
                }
                Type btype = type;

                //if ServiceAttribute type set, base type will be specified base type.
                if (attr.BaseType != null && type.IsAssignableFrom(attr.BaseType)) btype = attr.BaseType;
                else //otherwise. start to scan interfaces to determine base type.
                {
                    //interface scanning default condition: same namespace or same assembly.
                    Type inf = type.GetInterfaces().FirstOrDefault(t => t.Namespace == type.Namespace || t.GetTypeInfo().Assembly.GetName() == info.Assembly.GetName());
                    this.log.LogDebug($"Scanned base type: {inf?.FullName ?? "(itself)"}");
                    if (inf != null) btype = inf;
                }

                if (type != btype)
                {
                    ScanDescriptor desc;
                    if (!collect.TryGetValue(btype, out desc)) collect.Add(btype, desc = new ScanDescriptor());

                    desc.Types.Add(new Tuple<Type, ServiceAttribute>(type, attr));
                }
                else this.Register(collection, type, attr.Lifetime, info.GetCustomAttributes<LazyAttribute>().Any());
            }
            //https://github.com/khellang/Scrutor
            if (collect.Count > 0)
            {
                this.log.LogDebug($"{collect.Count} based Service type will be registered.");
                ServiceDescriptor desc;
                foreach (var kv in collect)
                {
                    if (kv.Value.Types.Count == 0) continue;
                    kv.Value.Sort();
                    List<ServiceDescriptor> results = new List<ServiceDescriptor>(kv.Value.Types.Count);
                    var keyInfo = kv.Key.GetTypeInfo();
                    var isGen = keyInfo.IsGenericType && keyInfo.IsGenericTypeDefinition;
                    foreach (var tuple in kv.Value.Types)
                    {
                        var valInfo = tuple.Item1.GetTypeInfo();
                        var item2 = tuple.Item2 as ServiceFlagAttribute;
                        var isLazy = valInfo.GetCustomAttributes<LazyAttribute>().Any();
                        var rktype = isGen ? kv.Key.MakeGenericType(valInfo.GenericTypeArguments) : kv.Key;
                        this.log.LogDebug($"adding base type: {rktype}");
                        this.log.LogDebug($"adding body type: {tuple.Item1}");
                        if (item2 != null)
                        {
                            if (!item2.TryDescribe(rktype, tuple.Item1, results, out desc))
                            {
                                if (desc != null) results.Add(this.AddDescription(isLazy ? ServiceUtilities.LazyDescribe(desc) : desc));
                                break;
                            }
                            if (desc != null) results.Add(this.AddDescription(isLazy ? ServiceUtilities.LazyDescribe(desc) : desc));
                        }
                        else results.Add(this.AddDescription(rktype, tuple.Item1, tuple.Item2.Lifetime, isLazy));
                    }

                    if (results.Count == 0) continue;
                    collection.TryAdd(results[results.Count - 1]);
                    results.RemoveAt(results.Count - 1);
                    if (results.Count == 0) continue;
                    this.log.LogDebug($"multiple types from a base type detected. adding {results.Count} type(s) will be registered in {typeof(IEnumerable<>).MakeGenericType(kv.Key)}");
                    for (int i = 0, len = results.Count; i < len; i++) results[i] = ServiceDescriptor.Describe(results[i].ImplementationType, results[i].ImplementationType, results[i].Lifetime);
                    results.Capacity = results.Count;
                    collection.TryAdd(results);
                    collection.TryAddScoped(typeof(IEnumerable<>).MakeGenericType(kv.Key), p => { return results.Select(d => p.GetService(d.ImplementationType)); });
                }
            }

            this.log.LogInformation($"${this.scanned.Count} type(s) has been added by ServiceAttribute.");
        }

        private ServiceDescriptor AddDescription(Type baseType, Type bodyType, ServiceLifetime lifetime, bool isLazy)
        {
            var desc = isLazy ? ServiceUtilities.LazyDescribe(baseType, bodyType, lifetime) : ServiceDescriptor.Describe(baseType, bodyType, lifetime);
            if (!this.scanned.ContainsKey(baseType)) this.scanned.Add(baseType, new List<ServiceDescriptor>() { desc });
            else this.scanned[baseType].Add(desc);
            return desc;
        }

        private ServiceDescriptor AddDescription(ServiceDescriptor descriptor)
        {
            if (!this.scanned.ContainsKey(descriptor.ServiceType)) this.scanned.Add(descriptor.ServiceType, new List<ServiceDescriptor>() { descriptor });
            else this.scanned[descriptor.ServiceType].Add(descriptor);
            return descriptor;
        }

        private void Register(IServiceCollection collection, Type baseType, Type bodyType, ServiceLifetime lifetime, bool isLazy)
        {
            collection.TryAdd(this.AddDescription(baseType, bodyType, lifetime, isLazy));
        }

        private void Register(IServiceCollection collection, Type bodyType, ServiceLifetime lifetime, bool isLazy)
        {
            collection.TryAdd(this.AddDescription(bodyType, bodyType, lifetime, isLazy));
        }
    }
}