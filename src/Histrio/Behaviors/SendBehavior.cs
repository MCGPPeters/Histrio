using Histrio.Commands;

namespace Histrio.Behaviors
{
    public class SendBehavior : BehaviorBase
    {
        private readonly IAddress _address;

        public SendBehavior(IAddress address)
        {
            _address = address;
        }

        public override void Accept<T>(Message<T> message)
        {
            Send.Message(message).To(_address);
        }
    }
}