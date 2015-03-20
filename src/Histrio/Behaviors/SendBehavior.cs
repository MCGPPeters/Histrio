namespace Histrio.Behaviors
{
    public class SendBehavior : BehaviorBase
    {
        private readonly IAddress _address;

        public SendBehavior(IAddress address)
        {
            _address = address;
        }

        protected override void AcceptCore<T>(IMessage<T> message)
        {
            var forwardedMessage = message.To(_address);
            Send.Message(forwardedMessage);
        }
    }
}