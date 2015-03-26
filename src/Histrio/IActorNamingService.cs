using System;

namespace Histrio
{
    public interface IActorNamingService
    {
        Uri ResolveActorLocation(IAddress address);
        void Register(IAddress address, Uri universalActorLocation);
    }
}