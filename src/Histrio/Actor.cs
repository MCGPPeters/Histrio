using Histrio.Behaviors;

namespace Histrio
{
    internal class Actor : IActor
    {
        private BehaviorBase _behavior;

        public Actor(BehaviorBase behavior, IAddress address)
        {
            _behavior = behavior;
            Address = address;
            _behavior.Actor = this;
        }

        public void Become(IAddress address)
        {
            _behavior = new SendBehavior(address);
        }

        public IAddress Address { get; private set; }

        public void Accept<TMessage>(TMessage message)
        {
            _behavior.Accept(message.InEnvelope());
        }
    }
}