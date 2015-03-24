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
            message.To = _address;
            Actor.Send(message);
        }
    }
}