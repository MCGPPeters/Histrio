namespace Histrio.Behaviors
{
    /// <summary>
    /// A behavior that simply forwards message to an address of another actor
    /// </summary>
    public class SendBehavior : BehaviorBase
    {
        private readonly Address _address;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBehavior"/> class.
        /// </summary>
        /// <param name="address">The address the behavior fill forward messages to</param>
        public SendBehavior(Address address)
        {
            _address = address;
        }

        /// <summary>
        /// Overrides the behavior of the BehaviorBase, so that it doesn't check if it can handle the message.
        /// It doesn't need to know the type of the message because it only forwards it
        /// </summary>
        /// <typeparam name="T">The type of content the message enbodies</typeparam>
        /// <param name="message">The message.</param>
        public override void Accept<T>(Message<T> message)
        {
            message.To = _address;
            Actor.Send(message);
        }
    }
}