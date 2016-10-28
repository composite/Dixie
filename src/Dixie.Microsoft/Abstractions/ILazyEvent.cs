namespace Dixie.Microsoft.Abstractions
{
    /// <summary>
    /// If implemented class have interited with this interface, this class will can be with lazy event.
    /// </summary>
    public interface ILazyEvent
    {
        /// <summary>
        /// Fires when instance created.
        /// </summary>
        void OnCreate();

        /// <summary>
        /// Fires when getting existing instance.
        /// </summary>
        void OnValue();
    }
}