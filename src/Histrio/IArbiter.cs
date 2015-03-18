using System;

namespace Histrio
{
    internal interface IArbiter : IDisposable
    {
        void Decide<T>(T message);
        void Start(IAccept actor);
    }
}