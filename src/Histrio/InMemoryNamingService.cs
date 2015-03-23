using System;
using System.Collections.Generic;

namespace Histrio
{
    public class InMemoryNamingService : IActorNamingService
    {
        private readonly Dictionary<Uri, Uri> _addresses = new Dictionary<Uri, Uri>();

        public Uri ResolveActorLocation(Uri universalActorNameOfRemoteActor)
        {
            return _addresses[universalActorNameOfRemoteActor];
        }

        public void Register(Uri universalActorName, Uri universalActorLocation)
        {
            _addresses.Add(universalActorName, universalActorLocation);
        }
    }
}