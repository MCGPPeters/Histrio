using Histrio.Behaviors;

namespace Histrio
{
    public interface IActor
    {
        IAddress Address { get; }
        void Become(IAddress address);
        IAddress Create(BehaviorBase behavior);
        void Send<T>(Message<T> message);
    }
}