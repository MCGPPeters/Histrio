namespace Histrio
{
    public interface IActor
    {
        IAddress Address { get; }
        void Become(IAddress address);
    }
}