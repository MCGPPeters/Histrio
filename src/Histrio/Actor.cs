using System.Threading.Tasks;
using Histrio.Behaviors;

namespace Histrio
{
    internal class Actor : IActor
    {
        private BehaviorBase _behavior;

        public Actor(BehaviorBase behavior, IAddress address, IArbiter arbiter)
        {
            _behavior = behavior;
            Address = address;
            _behavior.Actor = this;
            new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
                .StartNew(() =>
                {
                    var messages = arbiter.MailBox;
                    foreach (var message in messages.GetConsumingEnumerable())
                    {
                        message.GetHandledBy(_behavior);
                    }
                });
        }

        public void Become(IAddress address)
        {
            _behavior = new SendBehavior(address);
        }

        public IAddress Address { get; }
    }
}