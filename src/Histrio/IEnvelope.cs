using System.Threading.Tasks;

namespace Histrio
{
    public interface IEnvelope<out T>
    {
        T Body { get; }

        void GetHandledBy(IHandle<T> behavior);
    }
}