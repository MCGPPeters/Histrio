using System.Threading.Tasks;

using Histrio.Behaviors;

namespace Histrio.Tests.Bus
{
    public abstract class BusBehaviorBase : BehaviorBase, IHandle<Publish>, IHandle<Subscribe>
    {
        public abstract Task Accept(Publish message);

        public abstract Task Accept(Subscribe message);
    }
}