using System.Threading.Tasks;
using Histrio.Logging;

namespace Histrio
{
    internal class Actor : IActor
    {
        private static readonly ILog Logger = LogProvider.For<Actor>();

        public static Address Create(Behavior behavior, string name, MailBox mailBox, Theater theater)
        {
            var address = new Address(name);
            new Actor(behavior, address, mailBox, theater);
            return address;
        }

        private Behavior _behavior;

        private Actor(Behavior behavior, Address address, MailBox mailBox, Theater theater)
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

            Logger.DebugFormat("Sent message of type '{0}' from actor with behavior of type '{1}' at address '{2}' to an actor at address '{3}'",
                typeof(T), _behavior.GetType().Name, Address, message.To);

            Logger.TraceFormat("Sent message contents : {@message}", message.Body);
        }

        public void Send<T>(T messageContent, Address to)
        {
            var message = messageContent.AsMessage();
            message.To = to;
            Send(message);
        }

        public void Send<T>(T messageContent, string actorName)
        {
            Send(messageContent, new Address(actorName));
        }

        public void Become(Address address)
        {
            _behavior = new SendBehavior(address) {Actor = this};
        }

        public Address Create(Behavior behavior)
        {
            return Theater.CreateActor(behavior);
        }

        public Address Address { get; private set; }
    }
}