using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Histrio
{
    public class Address : IAddress
    {
        public Uri Uri { get; private set; }
        private readonly BlockingCollection<ICell> _messageBuffer = new BlockingCollection<ICell>();
        private readonly Dispatcher _dispatcher;

        public Address(Uri uri)
        {
            Uri = uri;
        }

        //public Address)
        //{
        //    //_dispatcher = dispatcher;
        //    //new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
        //    //   .StartNew(() =>
        //    //   {
        //    //       foreach (var genericObject in _messageBuffer.GetConsumingEnumerable())
        //    //       {
        //    //           genericObject.SendValueTo(_dispatcher);
        //    //       }
        //    //   });
        //}

        public void Receive<T>(T message)
        {
            //var genericObject = new Cell<IMessage<T>>();
            //var envelope = message.AsMessage();
            //envelope.Address = this;
            //genericObject.Set(envelope);
            //_messageBuffer.Add(genericObject);
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
                _messageBuffer.Dispose();
            }
        }
    }
}