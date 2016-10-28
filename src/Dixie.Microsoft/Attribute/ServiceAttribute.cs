namespace Dixie.Microsoft.Attribute
{
    using global::Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// This class will be used as service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        /// <summary>
        /// Initialize the service with options
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        /// <param name="life">The service lifetime</param>
        public ServiceAttribute(Type type, ServiceLifetime life)
        {
            this.BaseType = type;
            this.Lifetime = life;
        }

        /// <summary>
        /// Initialize the service with service lifetime.
        /// </summary>
        /// <param name="life">The service lifetime</param>
        public ServiceAttribute(ServiceLifetime life) : this(null, life) { }

        /// <summary>
        /// Initialize the service with base type.
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        public ServiceAttribute(Type type) : this(type, ServiceLifetime.Transient) { }

        /// <summary>
        /// Initialize the service with always created lifetime.
        /// </summary>
        public ServiceAttribute() : this(null, ServiceLifetime.Transient) { }

        /// <summary>
        /// Gets the base type.
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// Gets the service lifetime.
        /// </summary>
        public ServiceLifetime Lifetime { get; }
    }
}