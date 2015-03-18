using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Histrio
{
    internal class MailboxArbiter : IArbiter
    {
        private readonly BlockingCollection<ICell> _mailBox;

        public MailboxArbiter(int mailboxSize)
        {
            _mailBox  = new BlockingCollection<ICell>(mailboxSize);
        }

        public void Start(IAccept actor)
        {
            new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
                .StartNew(() =>
                {
                    try
                    {
                        foreach (var genericObject in _mailBox.GetConsumingEnumerable())
                        {
                            genericObject.SendValueTo(actor);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    finally
                    {
                        _mailBox.CompleteAdding();
                    }
                });
        }

        public void Decide<T>(T message)
        {
            var genericObject = new Cell<T>();
            genericObject.Set(message);
            _mailBox.Add(genericObject);
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
                _mailBox.Dispose();
            }
        }
    }
}