using System.Threading.Tasks;

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
            address.Subscribe(this);
            _behavior.Actor = this;
        }

        public void Become(IAddress address)
        {
            _behavior = new SendBehavior(address);
        }

        public IAddress Address { get; private set; }

        public async Task Accept<TMessage>(IMessage<TMessage> message)
        {
            await _behavior.Accept(message);
        }
    }
}