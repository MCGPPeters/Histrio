using System;
using System.Collections.Concurrent;

namespace Histrio
{
    internal sealed class MailBox : IDisposable
    {
        public MailBox(BlockingCollection<IMessage> blockingCollection)
        {
            Messages = blockingCollection;
        }

        public BlockingCollection<IMessage> Messages { get; private set; }

        public void Add(IMessage message)
        {
            Messages.Add(message);
        }

        bool _disposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Messages.Dispose();
            }

            // Free any unmanaged objects here. 
            //
            _disposed = true;
        }

        /// <summary>
        /// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MailBox()
        {
            Dispose(false);
        }
    }
}