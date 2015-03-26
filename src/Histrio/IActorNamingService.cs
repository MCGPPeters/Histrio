using System;

namespace Histrio
{
    public interface IActorNamingService
    {
        Uri ResolveActorLocation(Address address);
        void Register(Address address, Uri universalActorLocation);
    }
}