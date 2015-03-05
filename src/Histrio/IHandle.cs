using System.Threading.Tasks;

namespace Histrio
{
    public interface IHandle<in T>
    {
        Task Accept(T message);
    }
}