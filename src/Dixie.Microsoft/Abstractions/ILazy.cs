namespace Dixie.Microsoft.Abstractions
{
    /// <summary>
    /// The service object that only store type infomation and make service instance later.
    /// </summary>
    /// <remarks>If its class has inherited with <see cref="ILazyEvent"/>, lazy system will fire defined event delegation.</remarks>
    /// <typeparam name="T"></typeparam>
    public interface ILazy<out T> where T : class
    {
        /// <summary>
        /// Gets the type instance value. it will also create instance if not initialized.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// If service type initializer have non-service parameters, it can make instance with determined parameters.
        /// </summary>
        /// <param name="param">user-defined parameters.</param>
        /// <returns></returns>
        T Create(params object[] param);
    }
}