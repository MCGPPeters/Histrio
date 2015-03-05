using System.Threading.Tasks;

namespace Histrio
{
    public interface IAddress
    {
        Task Receive<T>(T message);

        void Subscribe(IActor actor);
    }
}