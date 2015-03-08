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
        private readonly Actor _actor;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly BlockingCollection<ICell> mailBox;

        public Address(int mailboxSize, BehaviorBase behavior)
        {
            _actor = new Actor(behavior, this);
            var cancellationToken = cancellationTokenSource.Token;
            mailBox = new BlockingCollection<ICell>(mailboxSize);
            new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
                .StartNew(() =>
                {
                    try
                    {
                        foreach (var genericObject in mailBox.GetConsumingEnumerable()
                            .TakeWhile(genericObject => !cancellationToken.IsCancellationRequested))
                        {
                            genericObject.SendValueTo(_actor);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    finally
                    {
                        mailBox.CompleteAdding();
                    }
                });
        }

        public void Receive<T>(T message)
        {
            var genericObject = new Cell<T>();
            genericObject.Set(message);
            mailBox.Add(genericObject, cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            mailBox.CompleteAdding();
            mailBox.Dispose();
            cancellationTokenSource.Dispose();
        }
    }
}