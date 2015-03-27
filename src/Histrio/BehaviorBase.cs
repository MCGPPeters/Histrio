using System;

namespace Histrio
{
    /// <summary>
    /// A base class for implementing behaviors that get injected into Actors
    /// </summary>
    public abstract class BehaviorBase
    {
        /// <summary>
        /// Gets or sets the actor the behavior is injected to. Use this reference to create new Actors,
        /// send messages to other Actors and replace this Actor with a new one
        /// </summary>
        /// <value>
        /// The actor.
        /// </value>
        protected internal IActor Actor { protected get; set; }

        /// <summary>
        /// Accepts the specified message and checkes if the message can be handled by this behavior.
        /// If so, it notifies the message it can be handled. It uses a double dispatch to let the message
        /// handle itself in s strongly typed fashion by the behavior
        /// </summary>
        /// /// <typeparam name="T">The type of content the message enbodies</typeparam>
        /// <param name="message">The message.</param>
        /// <exception cref="InvalidOperationException">The exception is thrown when the behavior can not handle this type of message content</exception>
        public virtual void Accept<T>(Message<T> message)
        {
            var handler = this as IHandle<T>;
            if (handler != null)
            {
                message.GetHandledBy(handler);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}