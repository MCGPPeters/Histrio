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

        public IAddress Address { get; private set; }
        public void GetHandledBy(BehaviorBase behavior)
        {
            behavior.Accept(this);
        }

        public void GetHandledBy(IHandle<T> behavior)
        {
            behavior.Accept(Body);
        }

        public void To(IAddress address)
        {
            Address = address;
            if (address.Uri.Scheme == "actor")
            {
                Context.System.Dispatch(this);
            }
        }
    }
}