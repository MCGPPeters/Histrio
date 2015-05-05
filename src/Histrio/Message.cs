using Histrio.Logging;

namespace Histrio
{
    /// <summary>
    ///     A message that carries a typed payload anabling strongly type message handling
    ///     using POCO (Plain Old CLR Objects).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Message<T> : IMessage
    {
        private static readonly ILog Logger = LogProvider.For<Message<T>>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Message{T}" /> class.
        /// </summary>
        /// <param name="body">The body.</param>
        public Message(T body)
        {
            Body = body;
        }

        /// <summary>
        ///     Gets or sets the body.
        /// </summary>
        /// <value>
        ///     The body.
        /// </value>
        public T Body { get; private set; }

        /// <summary>
        ///     Gets or sets the <see cref="Address" /> of the Actor that is the recipient of this message
        /// </summary>
        /// <value>
        ///     To.
        /// </value>
        public Address To { get; set; }

        /// <summary>
        ///     Gets the handled by.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void GetHandledBy(BehaviorBase behavior)
        {
            behavior.Accept(this);
        }

        /// <summary>
        ///     Gets the handled by.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        internal void GetHandledBy(IHandle<T> behavior)
        {
            behavior.Accept(Body);

            Logger.DebugFormat("A message of type '{0}' has been accepted by a behavior of type '{1}'",
                typeof(T), behavior.GetType().Name);

            Logger.TraceFormat("Accepted message contents : {@message}", Body);
        }
    }
}