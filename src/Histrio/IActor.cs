namespace Histrio
{
    public interface IActor : IAccept
    {
        IAddress Address { get; }
        void Become(IAddress address);
    }
}