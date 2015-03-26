using Histrio.Behaviors;

namespace Histrio
{
    public interface IMessage
    {
        Address To { get; }
        void GetHandledBy(BehaviorBase behavior);
    }
}