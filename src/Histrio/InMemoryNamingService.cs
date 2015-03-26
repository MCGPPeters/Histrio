using System;
using System.Collections.Generic;

namespace Histrio
{
    public class InMemoryNamingService : IActorNamingService
    {
        private readonly Dictionary<Address, Uri> _addresses = new Dictionary<Address, Uri>();

        public Uri ResolveActorLocation(Address address)
        {
            return _addresses[address];
        }

        public void Register(Address address, Uri universalActorLocation)
        {
            _addresses.Add(address, universalActorLocation);
        }
    }
}