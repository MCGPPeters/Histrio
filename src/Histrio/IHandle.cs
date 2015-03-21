namespace Histrio
{
    public interface IHandle
    {
        void Accept<T>(Message<T> message);
    }

    public interface IHandle<in T>
    {
        void Accept(T message);
    }
}