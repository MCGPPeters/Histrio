using System.Threading.Tasks;

namespace Histrio
{
    public interface IMessage<out T>
    {
        T Body { get; }

        Task GetHandledBy(IHandle<T> behavior);
    }
}