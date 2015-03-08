namespace Histrio
{
    internal interface ICell
    {
        void SendValueTo(IAccept receiver);
    }
}