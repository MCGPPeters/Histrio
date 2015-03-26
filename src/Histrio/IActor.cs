using Histrio.Behaviors;

namespace Histrio
{
    public interface IActor
    {
        Address Address { get; }
        void Become(Address address);
        Address Create(BehaviorBase behavior);
        void Send<T>(Message<T> message);
    }
}