using System;

namespace Histrio
{
    public interface IAddress : IDisposable
    {
        void Receive<T>(T message);
        Uri Uri { get; }
    }
}