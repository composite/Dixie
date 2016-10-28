namespace Dixie.Microsoft
{
    using Dixie.Microsoft.Abstractions;
    using Dixie.Microsoft.Util;
    using global::Microsoft.Extensions.DependencyInjection;
    using global::Microsoft.Extensions.DependencyInjection.Extensions;
    using global::Microsoft.Extensions.DependencyModel;
    using System;
    using System.Reflection;

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
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, new object[] { new[] { baseType.GetTypeInfo().Assembly } })));
            return collection;
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, params Type[] types)
        {
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, new object[] { types })));
            return collection;
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, params Assembly[] assemblies)
        {
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, new object[] { assemblies })));
            return collection;
        }

        public static IServiceCollection AddServiceScan(this IServiceCollection collection, DependencyContext context)
        {
            collection.TryAddSingleton(collection.GetScanService(provider => ActivatorUtilities.CreateInstance<ScanService>(provider, context)));
            return collection;
        }

        public static IServiceCollection AddLazy(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            collection.Add(ServiceUtilities.LazyDescribe(descriptor));
            return collection;
        }

        public static IServiceCollection TryAddLazy(this IServiceCollection collection, ServiceDescriptor descriptor)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(descriptor));
            return collection;
        }

        public static IServiceCollection AddLazyTransient(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyTransient<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> objectFactory) where TService : class
        {
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection TryAddLazyTransient<TService>(this IServiceCollection collection, Func<IServiceProvider, TService> objectFactory) where TService : class
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Transient));
            return collection;
        }

        public static IServiceCollection AddLazyScoped(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazyScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazyScoped<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazyScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection TryAddLazyScoped<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Scoped));
            return collection;
        }

        public static IServiceCollection AddLazySingleton(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddLazySingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            collection.Add(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddLazySingleton<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection AddLazySingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            collection.Add(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton(this IServiceCollection collection, Type serviceType, Type implementationType)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, implementationType, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton(this IServiceCollection collection, Type serviceType, Func<IServiceProvider, object> objectFactory)
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(serviceType, objectFactory, ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton<TService, TImpl>(this IServiceCollection collection) where TService : class where TImpl : TService
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), typeof(TImpl), ServiceLifetime.Singleton));
            return collection;
        }

        public static IServiceCollection TryAddLazySingleton<TService>(this IServiceCollection collection, Func<IServiceProvider, object> objectFactory) where TService : class
        {
            collection.TryAdd(ServiceUtilities.LazyDescribe(typeof(TService), objectFactory, ServiceLifetime.Singleton));
            return collection;
        }
    }
}