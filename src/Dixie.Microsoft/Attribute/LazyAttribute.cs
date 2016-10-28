namespace Dixie.Microsoft.Attribute
{
    using System;

    /// <summary>
    /// This service will be called later. this class can accessed with <see cref="ILazy{T}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LazyAttribute : Attribute
    {
    }
}