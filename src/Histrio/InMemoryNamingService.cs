using System;
using System.Collections.Generic;

namespace Histrio
{
    public class InMemoryNamingService : IActorNamingService
    {
        private readonly Dictionary<IAddress, Uri> _addresses = new Dictionary<IAddress, Uri>();

        public Uri ResolveActorLocation(IAddress address)
        {
            return _addresses[address];
        }

        public void Register(IAddress address, Uri universalActorLocation)
        {
            _addresses.Add(address, universalActorLocation);
        }
    }
}