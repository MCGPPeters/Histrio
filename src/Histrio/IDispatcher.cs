using System;

namespace Histrio
{
    public interface IDispatcher
    {
        bool CanDispathFor(Uri universalActorLocation);
        void Dispatch<T>(Message<T> message, Uri universalActorLocation);
    }
}