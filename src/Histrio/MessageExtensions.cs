namespace Histrio
{
    /// <summary>
    /// Extension methods for <see cref="Message{T}"/>
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Convenience method to wrap a POCO into a message
        /// </summary>
        /// <typeparam name="T">The type of the POCO</typeparam>
        /// <param name="body">The object to wrap</param>
        /// <returns>A new message</returns>
        public static Message<T> AsMessage<T>(this T body)
        {
            return new Message<T>(body);
        }
    }
}