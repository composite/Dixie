namespace Dixie.Microsoft
{
    using Dixie.Microsoft.Abstractions;
    using Dixie.Microsoft.Util;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.DependencyInjection.Extensions;
    using global::Microsoft.Extensions.DependencyModel;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Dixie.Microsoft.Attribute;
    using Dixie.Microsoft.Internal;

    public static class ServiceExtensions
    {
        private static IScanService GetScanService(this IServiceCollection collection, Func<IServiceProvider, IScanService> init)
        {
            IServiceCollection builder = new ServiceCollection();
            builder.AddLogging().AddSingleton(collection).AddSingleton(init);
            IServiceProvider build = builder.BuildServiceProvider();
            return build.GetRequiredService<IScanService>();
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, Type baseType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (baseType == null) throw new ArgumentNullException(nameof(baseType));
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, new object[] { new[] { baseType.GetTypeInfo().Assembly } })));
            return collection;
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, params Type[] types)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (types == null) throw new ArgumentNullException(nameof(types));
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, new object[] { types })));
            return collection;
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, params Assembly[] assemblies)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, new object[] { assemblies })));
            return collection;
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, DependencyContext context)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (context == null) throw new ArgumentNullException(nameof(context));
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, context)));
            return collection;
        }

        public static IServiceCollection AddLazy(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            collection.Add(ServiceUtilities.LazyDescribe(descriptor));
            return collection;
        }

        private static ServiceDescriptor AddLazyAndSelf(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            AddLazy(collection, descriptor);
            return descriptor;
        }

        public static IServiceCollection TryAddLazy(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (descriptor == null) throw new ArgumentNullException(nameof(descriptor));
            collection.TryAdd(ServiceUtilities.LazyDescribe(descriptor));
            return collection;
        }

        public static IServiceCollection AddLazyTransient(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyTransient<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> objectFactory) where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> objectFactory) where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyScoped(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazyScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazyScoped<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazyScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazySingleton(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddLazySingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddLazySingleton<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddLazySingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (objectFactory == null) throw new ArgumentNullException(nameof(objectFactory));
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddServicePluggable<TFlag>(this IServiceCollection collection, TFlag flag, Type serviceType, Type bodyType, bool lazy = false) where TFlag : ServiceFlagAttribute
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (flag == null) throw new ArgumentNullException(nameof(flag));
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (bodyType == null) throw new ArgumentNullException(nameof(bodyType));

            var sinfo = serviceType.GetTypeInfo();
            var vinfo = bodyType.GetTypeInfo();

            if (vinfo.IsGenericType && vinfo.IsGenericTypeDefinition) throw new NotSupportedException($"implementable definition types are not supported: {bodyType}");
            if (sinfo.IsGenericType && sinfo.IsGenericTypeDefinition) serviceType = serviceType.MakeGenericType(vinfo.GenericTypeArguments);

            using (var dist = (IDisposable)Activator.CreateInstance(typeof(PluggableServiceCollection<>).MakeGenericType(serviceType), collection))
            {
                var descs = (IList<ServiceDescriptor>)dist;
                ServiceDescriptor desc;
                if (!flag.TryDescribe(serviceType, bodyType, descs, out desc))
                {
                    if (desc != null) descs.Add(lazy ? collection.AddLazyAndSelf(desc) : desc);
                    return collection;
                }
                if (desc != null) descs.Add(lazy ? collection.AddLazyAndSelf(desc) : desc);
            }

            return collection;
        }

        public static IServiceCollection AddServiceDecorator(this IServiceCollection collection, Type serviceType, Type bodyType)
        {
            return AddServicePluggable(collection, new DecoratorAttribute(), serviceType, bodyType);
        }

        public static IServiceCollection AddServiceDecorator<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            return AddServicePluggable(collection, new DecoratorAttribute(), typeof(TService), typeof(TImpl));
        }
    }
}