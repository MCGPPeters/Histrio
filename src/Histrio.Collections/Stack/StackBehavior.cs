using System.Threading.Tasks;

using Histrio.Behaviors;

namespace Histrio.Collections.Stack
{
    public class StackBehavior<T> : BehaviorBase, IHandle<Push<T>>, IHandle<Pop>
    {
        private readonly T content;
        private readonly IAddress link;

        public StackBehavior(T content, IAddress link)
        {
            this.content = content;
            this.link = link;
        }

        public async Task Accept(Pop message)
        {
            Actor.Become(link);
            await message.Customer.Receive(content);
        }

        public Task Accept(Push<T> message)
        {
            var p = System.AddressOf(new StackBehavior<T>(content, link));
            Actor.Become(System.AddressOf(new StackBehavior<T>(message.Value, p)));
            return Task.FromResult(false);
        }
    }
}

