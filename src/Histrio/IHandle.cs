namespace Histrio
{
    public interface IHandle
    {
        void Accept(IMessage message);
    }

    public interface IHandle<in T>
    {
        void Accept(T message);
    }
}