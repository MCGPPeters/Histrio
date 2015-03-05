using System.Threading.Tasks;

namespace Histrio
{
    public interface IActor
    {
        IAddress Address { get; }

        void Become(IAddress address);

        Task Accept<TMessage>(IMessage<TMessage> message);
    }
}