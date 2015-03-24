using Histrio.Behaviors;

namespace Histrio
{
    public interface IMessage
    {
        IAddress To { get; }
        void GetHandledBy(BehaviorBase behavior);
    }
}