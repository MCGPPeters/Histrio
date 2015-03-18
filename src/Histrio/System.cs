using System.Collections.Generic;
using Histrio.Behaviors;
using Microsoft.Practices.ServiceLocation;

namespace Histrio
{
    public sealed class System
    {
        private readonly IServiceLocator _serviceLocator;
        private readonly List<IAddress> _addresses = new List<IAddress>();

        public System(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public System() : this(ServiceLocator.Current)
        {
            
        }

        public IAddress AddressOf(BehaviorBase behavior)
        {
            behavior.System = this;
            var address = new Address(behavior, new MailboxArbiter(mailboxSize: 32));
            _addresses.Add(address);
            return address;
        }

        public IAddress AddressOf<T>() where T : BehaviorBase
        {
            var behavior = _serviceLocator.GetInstance<T>();
            return AddressOf(behavior);
        }
    }
}