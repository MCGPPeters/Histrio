using Histrio.Behaviors;

namespace Histrio
{
    public class Message<T> : IMessage
    {
        public Message(T body)
        {
            Body = body;
        }

        public T Body { get; set; }
        public IAddress To { get; set; }

        public void GetHandledBy(BehaviorBase behavior)
        {
            behavior.Accept(this);
        }

        public void GetHandledBy(IHandle<T> behavior)
        {
            behavior.Accept(Body);
        }
    }
}