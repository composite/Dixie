namespace Dixie.Microsoft.Attribute
{
    using global::Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// This class will be used as repository service. lifetime will be same as <see cref="ServiceLifetime.Scoped"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RepositoryAttribute : ServiceAttribute
    {
        /// <summary>
        /// Initialize the repository service with base type.
        /// </summary>
        /// <param name="type">The base type (if you want use class self, assign null.)</param>
        public RepositoryAttribute(Type type) : base(type, ServiceLifetime.Scoped) { }

        /// <summary>
        /// Initialize the repository service within self.
        /// </summary>
        public RepositoryAttribute() : base(null, ServiceLifetime.Scoped) { }
    }
}