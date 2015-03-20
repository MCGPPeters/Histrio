namespace Histrio
{
    public class Message<T> : IMessage<T>
    {
        public Message(T body)
        {
            Body = body;
        }

        public void GetHandledBy(IHandle<T> behavior)
        {
            behavior.Accept(Body);
        }

        public IMessage<T> To(IAddress address)
        {
            Address = address;
            return this;
        }

        public T Body { get; private set; }

        public IAddress Address
        {
            get; private set; }
    }
}