using System.Collections.Concurrent;

namespace Histrio
{
    internal interface IArbiter
    {
        BlockingCollection<IMessage> MailBox { get; }
    }
}