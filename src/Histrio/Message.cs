using System.Threading.Tasks;

namespace Histrio
{
    internal class Message<T> : IMessage<T>
    {
        public Message(T body)
        {
            Body = body;
        }

        public async Task GetHandledBy(IHandle<T> behavior)
        {
            await behavior.Accept(Body);
        }

        public T Body { get; private set; }
    }
}