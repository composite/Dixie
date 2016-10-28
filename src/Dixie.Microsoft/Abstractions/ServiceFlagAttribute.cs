namespace Dixie.Microsoft.Abstractions
{
    using Dixie.Microsoft.Attribute;
    using global::Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The service attribute but add service as pattern based.
    /// </summary>
    public abstract class ServiceFlagAttribute : ServiceAttribute
    {
        /// <summary>
        /// Initialize the service with options
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        /// <param name="life">The service lifetime</param>
        protected ServiceFlagAttribute(Type type, ServiceLifetime life) : base(type, life) { }

        /// <summary>
        /// Initialize the service with service lifetime.
        /// </summary>
        /// <param name="life">The service lifetime</param>
        protected ServiceFlagAttribute(ServiceLifetime life) : base(null, life) { }

        /// <summary>
        /// Initialize the service with base type.
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        protected ServiceFlagAttribute(Type type) : base(type, ServiceLifetime.Transient) { }

        /// <summary>
        /// Initialize the service with always created lifetime.
        /// </summary>
        protected ServiceFlagAttribute() : base(null, ServiceLifetime.Transient) { }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        public int Order { get; set; } = 0;

        /// <summary>
        /// Add refined candidates <see cref="ServiceDescriptor"/> collection. you can edit multiple <see cref="ServiceDescriptor"/> such as add or remove. returns bool for continue or break loop.
        /// </summary>
        /// <remarks>Only last interface based <see cref="ServiceDescriptor"/> will be added in candidates collection. other implements are added in <see cref="IEnumerable{T}"/></remarks>
        /// <param name="baseType">The discovered service type. don't use <see cref="ServiceAttribute.BaseType"/> for get the service type properly.</param>
        /// <param name="bodyType">The attribute defined actual type.</param>
        /// <param name="candidates">The editable service candidates collection. only first service will be added.</param>
        /// <param name="result">returns redefined <see cref="ServiceDescriptor"/> or null if would not add to candidates service collection.</param>
        /// <returns>returns bool for continue or break loop.</returns>
        public abstract bool TryDescribe(Type baseType, Type bodyType, IList<ServiceDescriptor> candidates, out ServiceDescriptor result);
    }
}