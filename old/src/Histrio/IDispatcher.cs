using System;

namespace Histrio
{
    /// <summary>
    ///     A dispatcher is used for sending messages to Actors in that are located in another Theater (for instance via HTTP)
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        ///     Determines whether this instance [can dispatch to] the specified actor location.
        /// </summary>
        /// <param name="actorLocation">The actor location.</param>
        /// <returns></returns>
        bool CanDispatchTo(Uri actorLocation);

        /// <summary>
        ///     Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T">The type of message to dispatch</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="actorLocation">The actor location.</param>
        void Dispatch<T>(Message<T> message, Uri actorLocation);
    }
}