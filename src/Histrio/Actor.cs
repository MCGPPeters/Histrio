using System.Threading.Tasks;
using Histrio.Behaviors;

namespace Histrio
{
    internal class Actor : IActor
    {
        private BehaviorBase _behavior;

        public Actor(BehaviorBase behavior, IAddress address, MailBox mailBox, Theater theater)
        {
            _behavior = behavior;
            Address = address;
            Theater = theater;
            _behavior.Actor = this;
            new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
                .StartNew(() =>
                {
                    var messages = mailBox.Messages;
                    foreach (var message in messages.GetConsumingEnumerable())
                    {
                        message.GetHandledBy(_behavior);
                    }
                });
        }

        private Theater Theater { get; set; }

        public void Send<T>(Message<T> message)
        {
            Theater.Dispatch(message);
        }

        public void Become(IAddress address)
        {
            _behavior = new SendBehavior(address) {Actor = this};
        }

        public IAddress Create(BehaviorBase behavior)
        {
            return Theater.CreateActor(behavior);
        }

        public IAddress Address { get; private set; }
    }
}