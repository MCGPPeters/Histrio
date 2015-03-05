using System.Threading.Tasks;

namespace Histrio.Behaviors.StorageCell
{
    public class StorageCellBehavior<T> : BehaviorBase, IHandle<Get>, IHandle<Set<T>>
    {
        private T _contents;

        public async Task Accept(Get message)
        {
            await message.Costumer.Receive(new Reply<T>(_contents));
        }

        public Task Accept(Set<T> message)
        {
            _contents = message.Body;
            return Task.FromResult(false);
        }
    }
}