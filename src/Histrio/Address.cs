using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Histrio.Behaviors;

namespace Histrio
{
    internal class Address : IAddress
    {
        private readonly IActor _actor;
        private readonly MailboxArbiter _mailboxArbiter;

        public Address(BehaviorBase behavior, MailboxArbiter mailboxArbiter)
        {
            _mailboxArbiter = mailboxArbiter;
            _actor = new Actor(behavior, this);
            _mailboxArbiter.Start(_actor);
        }

        public void Receive<T>(T message)
        {
            _mailboxArbiter.Decide(message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mailboxArbiter.Dispose();
            }
        }
    }
}