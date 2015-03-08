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

        protected internal override void AcceptCore<T>(IEnvelope<T> message)
        {
            _address.Receive(message.Body);
        }
    }
}