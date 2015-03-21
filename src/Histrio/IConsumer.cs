namespace Histrio
{
    public interface IConsumer
    {
        void Accept<T>(Message<T> message);
    }
}