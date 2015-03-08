using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Histrio
{
    public interface IAddress : IDisposable
    {
        void Receive<T>(T message);
    }
}