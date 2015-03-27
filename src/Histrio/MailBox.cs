using System;
using System.Collections.Concurrent;

namespace Histrio
{
    internal sealed class MailBox : IDisposable
    {
        private bool _disposed;

        internal MailBox(BlockingCollection<IMessage> blockingCollection)
        {
            Messages = blockingCollection;
        }

        internal BlockingCollection<IMessage> Messages { get; private set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void Add(IMessage message)
        {
            Messages.Add(message);
        }

        /// <summary>
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
        ///     Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage
        ///     collection.
        /// </summary>
        ~MailBox()
        {
            Dispose(false);
        }
    }
}