using System;

namespace Histrio
{
    public interface IActorNamingService
    {
        Uri ResolveActorLocation(Uri universalActorNameOfRemoteActor);
        void Register(Uri universalActorName, Uri universalActorLocation);
    }
}