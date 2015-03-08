using System.Threading.Tasks;

namespace Histrio.Behaviors.StorageCell
{
    public class StorageCellBehavior<T> : BehaviorBase, IHandle<Get>, IHandle<Set<T>>
    {
        private T _contents;

        public void Accept(Get message)
        {
            message.Costumer.Receive(new Reply<T>(_contents));
        }

        public void Accept(Set<T> message)
        {
            _contents = message.Body;
        }
    }
}