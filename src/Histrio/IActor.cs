namespace Histrio
{
    /// <summary>
    /// </summary>
    public interface IActor
    {
        /// <summary>
        ///     Gets the address of the actor
        /// </summary>
        /// <value>
        ///     The address.
        /// </value>
        Address Address { get; }

        /// <summary>
        ///     The become command, signifying that a replacement behavior will be used for the next message
        ///     that will arrive at the <see cref="Address" />
        /// </summary>
        /// <param name="address">The address.</param>
        void Become(Address address);

        /// <summary>
        ///     Creates a new actor with the specified behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        Address Create(BehaviorBase behavior);

        /// <summary>
        ///     Sends the specified message to the Address specified in the To property of the
        ///     <paramref name="message">message</paramref>
        /// </summary>
        /// <typeparam name="T">The type of (the content / body) the message carries</typeparam>
        /// <param name="message">The message.</param>
        void Send<T>(Message<T> message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageContent"></param>
        /// <param name="to"></param>
        /// <typeparam name="T"></typeparam>
        void Send<T>(T messageContent, Address to);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageContent"></param>
        /// <param name="actorName"></param>
        /// <typeparam name="T"></typeparam>
        void Send<T>(T messageContent, string actorName);
    }
}