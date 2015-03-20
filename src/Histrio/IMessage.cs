namespace Histrio
{
    public interface IMessage<out T>
    {
        T Body { get; }
        IAddress Address { get; }
        void GetHandledBy(IHandle<T> behavior);
        IMessage<T> To(IAddress address);
    }
}