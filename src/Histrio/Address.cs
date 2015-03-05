using System.Threading.Tasks;

using Histrio.Behaviors;

namespace Histrio
{
    internal class Address : IAddress
    {
        private IActor _actor;

        public async Task Receive<T>(T message)
        {
            await _actor.Accept(message.AsMessage());
        }

        public void Subscribe(IActor actor)
        {
            _actor = actor;
        }
    }
}