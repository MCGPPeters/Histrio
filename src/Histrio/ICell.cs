namespace Histrio
{
    internal interface ICell
    {
        void SendValueTo(IConsumer receiver);
    }
}