using System.Threading.Tasks;

namespace Histrio.Behaviors
{
    public class SendBehavior : BehaviorBase
    {
        private readonly IAddress _address;

        public SendBehavior(IAddress address)
        {
            _address = address;
        }

        protected override async Task AcceptCore<T>(IMessage<T> message)
        {
            await _address.Receive(message.Body);
        }
    }
}