using Histrio.Behaviors;

namespace Histrio
{
    public sealed class System
    {
        private readonly IContainer _container;

        public System(IContainer container)
        {
            _container = container;
        }

        public IAddress AddressOf(BehaviorBase behavior)
        {
            behavior.System = this;
            var address = new Address();
            new Actor(behavior, address);
            return address;
        }
        
        public IAddress AddressOf<T>() where T : BehaviorBase
        {
            var behavior = _container.Get<T>();
            return AddressOf(behavior);
        }
    }
}