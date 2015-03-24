using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Histrio
{
    internal sealed class MailboxArbiter : IArbiter
    {
        public MailboxArbiter(MailBox mailBox)
        {
            MailBox = new BlockingCollection<IMessage>();
            new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
                .StartNew(() =>
                {
                    var messages = mailBox.Messages;
                    foreach (var message in messages.GetConsumingEnumerable())
                    {
                        MailBox.Add(message);
                    }
                });
        }

        public BlockingCollection<IMessage> MailBox { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                MailBox.Dispose();
            }
        }
    }
}