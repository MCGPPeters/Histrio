namespace Histrio.Behaviors
{
    public class SendBehavior : BehaviorBase
    {
        private readonly Address _address;

        public SendBehavior(Address address)
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