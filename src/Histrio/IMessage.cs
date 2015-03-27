namespace Histrio
{
    internal interface IMessage
    {
        Address To { get; }
        void GetHandledBy(BehaviorBase behavior);
    }
}