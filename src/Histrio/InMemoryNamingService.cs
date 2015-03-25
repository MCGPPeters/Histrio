using System;
using System.Collections.Generic;

namespace Histrio
{
    public class InMemoryNamingService : IActorNamingService
    {
        private readonly Dictionary<Uri, Uri> _addresses = new Dictionary<Uri, Uri>();

        public Uri ResolveActorLocation(IAddress address)
        {
            return _addresses[address.UniversalActorName];
        }

        public void Register(Uri universalActorName, Uri universalActorLocation)
        {
            _addresses.Add(universalActorName, universalActorLocation);
        }
    }
}