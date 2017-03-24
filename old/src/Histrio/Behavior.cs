using System;
using Histrio.Logging;

namespace Histrio
{
    /// <summary>
    ///     A base class for implementing behaviors that get injected into Actors
    /// </summary>
    public abstract class Behavior
    {
        private static readonly ILog Logger = LogProvider.For<Behavior>();

        /// <summary>
        ///     Gets or sets the actor the behavior is injected to. Use this reference to create new Actors,
        ///     send messages to other Actors and replace this Actor with a new one
        /// </summary>
        /// <value>
        ///     The actor.
        /// </value>
        protected internal IActor Actor { protected get; set; }

        /// <summary>
        ///     Accepts the specified message and checkes if the message can be handled by this behavior.
        ///     If so, it notifies the message it can be handled. It uses a double dispatch to let the message
        ///     handle itself in s strongly typed fashion by the behavior
        /// </summary>
        /// ///
        /// <typeparam name="T">The type of content the message enbodies</typeparam>
        /// <param name="message">The message.</param>
        /// <exception cref="InvalidOperationException">
        ///     The exception is thrown when the behavior can not handle this type of
        ///     message content
        /// </exception>
        public virtual void Accept<T>(Message<T> message)
        {
            Logger.DebugFormat("A message of type '{0}' arrived at behavior of type '{1}' at address '{2}'",
                typeof(T).Name, GetType().Name, Actor.Address);

            Logger.TraceFormat("Message contents : {@message}", message.Body);

            var handler = this as IHandle<T>;
            if (handler != null)
            {
                message.GetHandledBy(handler);
            }
            else
            {
                var exceptionMessageFormat = "A message of type {0} cannot be handled by this behavior since it does not implement IHandle<{1}>";

                var messageType = typeof(T);
                var exceptionMessage = string.Format(exceptionMessageFormat, messageType.FullName, messageType.Name);
                var invalidOperationException = new InvalidOperationException(exceptionMessage);
                
                Logger.ErrorException(exceptionMessage, invalidOperationException);

                throw invalidOperationException;
            }
        }
    }
}