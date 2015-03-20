using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Histrio
{
    internal class MailboxArbiter : IArbiter
    {
        private readonly Dispatcher _dispatcher;
        private readonly BlockingCollection<ICell> _mailBox;

        public MailboxArbiter(int mailboxSize)
        {
            _mailBox  = new BlockingCollection<ICell>(mailboxSize);
        }

        public void Start(IAddress address)
        {

            //new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
            //    .StartNew(() =>
            //    {
            //        var messageBuffer = address.MessageBuffer;
            //        foreach (var genericObject in messageBuffer.GetConsumingEnumerable())
            //        {
            //            genericObject.SendValueTo(_dispatcher);
            //        }
            //    });
        }

        public void Decide<T>(T message)
        {
            var genericObject = new Cell<T>();
            genericObject.Set(message);
            _mailBox.Add(genericObject);
        }

        public void Start(IAccept actor)
        {
            throw new NotImplementedException();
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