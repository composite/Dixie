namespace Dixie.Microsoft.Util
{
    using Dixie.Microsoft.Abstractions;
    using Dixie.Microsoft.Internal;
    using global::Microsoft.Extensions.DependencyInjection;
    using System;

    internal static class ServiceUtilities
    {
        internal static ServiceDescriptor LazyDescribe(Type baseType, Type bodyType, ServiceLifetime lifetime)
        {
            return ServiceDescriptor.Describe(
                typeof(ILazy<>).MakeGenericType(baseType),
                p => ActivatorUtilities.CreateInstance(p, typeof(InternalLazyService<>).MakeGenericType(baseType), bodyType),
                lifetime);
        }

        internal static ServiceDescriptor LazyDescribe(Type baseType, Func<IServiceProvider, object> objectFactory, ServiceLifetime lifetime)
        {
            return ServiceDescriptor.Describe(
                typeof(ILazy<>).MakeGenericType(baseType),
                p => ActivatorUtilities.CreateInstance(p, typeof(InternalLazyService<>).MakeGenericType(baseType), objectFactory),
                lifetime);
        }

        internal static ServiceDescriptor LazyDescribe(ServiceDescriptor descriptor)
        {
            return ServiceDescriptor.Describe(
                    typeof(ILazy<>).MakeGenericType(descriptor.ServiceType),
                    p => ActivatorUtilities.CreateInstance(p, typeof(InternalLazyService<>).MakeGenericType(descriptor.ServiceType), (object)descriptor.ImplementationFactory ?? descriptor.ImplementationType),
                    descriptor.Lifetime);
        }
    }
}