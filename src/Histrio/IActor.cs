using System.Threading.Tasks;

namespace Histrio
{
    public interface IActor : IReference
    {
        IAddress Address { get; }

        void Become(IAddress address);
    }
}