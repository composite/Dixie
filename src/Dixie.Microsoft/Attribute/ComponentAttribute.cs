namespace Dixie.Microsoft.Attribute
{
    using global::Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// This class will be used as component service. lifetime will be same as <see cref="ServiceLifetime.Singleton"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ComponentAttribute : ServiceAttribute
    {
        /// <summary>
        /// Initialize the component service with base type.
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        public ComponentAttribute(Type type) : base(type, ServiceLifetime.Singleton) { }

        /// <summary>
        /// Initialize the component service within self.
        /// </summary>
        public ComponentAttribute() : base(null, ServiceLifetime.Singleton) { }
    }
}