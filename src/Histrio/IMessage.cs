using Histrio.Behaviors;

namespace Histrio
{
    public interface IMessage
    {
        IAddress Address { get; }
        void GetHandledBy(BehaviorBase behavior);
    }
}