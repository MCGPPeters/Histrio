using System;
using System.Collections.Generic;

namespace Histrio
{
    public interface IActorNamingService
    {
        Uri ResolveActorLocation(Uri universalActorNameOfRemoteActor);
        void Register(Uri universalActorName, Uri universalActorLocation);
    }
}