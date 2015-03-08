using System.Threading.Tasks;

namespace Histrio
{
    public interface IHandle<in T>
    {
        void Accept(T message);
    }
}