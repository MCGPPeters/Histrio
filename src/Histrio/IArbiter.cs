using System;
using System.Collections.Concurrent;

namespace Histrio
{
    internal interface IArbiter : IDisposable
    {
        BlockingCollection<IMessage> MailBox { get; }
    }
}