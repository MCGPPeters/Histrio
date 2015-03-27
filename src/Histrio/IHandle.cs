namespace Histrio
{
    /// <summary>
    /// An interface that is used to specify that a behavior can handle a specific type of message
    /// </summary>
    /// <typeparam name="T">The type of the message it can handle</typeparam>
    public interface IHandle<in T>
    {
        /// <summary>
        /// Accepts the specified message. Accept is Actor Model terminology for "I can do something with this message"
        /// </summary>
        /// <param name="message">The message.</param>
        void Accept(T message);
    }
}