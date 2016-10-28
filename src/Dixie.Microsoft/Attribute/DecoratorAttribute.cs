namespace Dixie.Microsoft.Attribute
{
    using Dixie.Microsoft.Abstractions;
    using global::Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The service flag attribute that will be used as decorator pattern based. only first service will be wrapped.
    /// </summary>
    public sealed class DecoratorAttribute : ServiceFlagAttribute
    {
        /// <summary>
        /// Initialize the service with options
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        /// <param name="life">The service lifetime</param>
        public DecoratorAttribute(Type type, ServiceLifetime life) : base(type, life) { }

        /// <summary>
        /// Initialize the service with service lifetime.
        /// </summary>
        /// <param name="life">The service lifetime</param>
        public DecoratorAttribute(ServiceLifetime life) : base(null, life) { }

        /// <summary>
        /// Initialize the service with base type.
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        public DecoratorAttribute(Type type) : base(type, ServiceLifetime.Transient) { }

        /// <summary>
        /// Initialize the service with always created lifetime.
        /// </summary>
        public DecoratorAttribute() : base(null, ServiceLifetime.Transient) { }

        /// <summary>
        /// process to decotate. only first service will be wrapped.
        /// </summary>
        /// <param name="baseType">The service type.</param>
        /// <param name="bodyType">The body type.</param>
        /// <param name="candidates">The editable candidates collection.</param>
        /// <returns></returns>
        public override bool TryDescribe(Type baseType, Type bodyType, IList<ServiceDescriptor> candidates, out ServiceDescriptor result)
        {
            if (candidates.Count == 0)
            {
                result = null;
                return true;
            }
            var desc = candidates[0];
            candidates.Clear();

            if (!baseType.IsAssignableFrom(bodyType)) throw new InvalidCastException($"Decorator '{bodyType}' is cannot service as '{baseType}'.");
            result = ServiceDescriptor.Describe(
                baseType,
                provider => ActivatorUtilities.CreateInstance(provider, bodyType, this.GetInstance(provider, desc)),
                this.Lifetime);
            return true;
        }

        private object GetInstance(IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null) return descriptor.ImplementationInstance;
            if (descriptor.ImplementationType != null) return ActivatorUtilities.GetServiceOrCreateInstance(provider, descriptor.ImplementationType);
            return descriptor.ImplementationFactory(provider);
        }
    }
}