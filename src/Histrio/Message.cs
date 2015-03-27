using Histrio.Behaviors;

namespace Histrio
{
    /// <summary>
    /// A message that carries a typed payload anabling strongly type message handling
    /// using POCO (Plain Old CLR Objects).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Message<T> : IMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Message{T}"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        public Message(T body)
        {
            Body = body;
        }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public T Body { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Address"/> of the Actor that is the recipient of this message
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public Address To { get; set; }

        /// <summary>
        /// Gets the handled by.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void GetHandledBy(BehaviorBase behavior)
        {
            behavior.Accept(this);
        }

        /// <summary>
        /// Gets the handled by.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void GetHandledBy(IHandle<T> behavior)
        {
            behavior.Accept(Body);
        }
    }
}