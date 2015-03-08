using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Histrio.Behaviors;

namespace Histrio
{
    internal class Address : IAddress
    {
        private readonly BlockingCollection<IObject> mailBox;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly Actor _actor;

        public Address(int mailboxSize, BehaviorBase behavior)
        {
            _actor = new Actor(behavior, this);
            var cancellationToken = cancellationTokenSource.Token;
            mailBox = new BlockingCollection<IObject>(mailboxSize);
                new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None)
                .StartNew(() =>
                {
                    try
                    {
                        foreach (var genericObject in mailBox.GetConsumingEnumerable()
                        .TakeWhile(genericObject => !cancellationToken.IsCancellationRequested))
                        {
                            genericObject.SendTo(_actor);
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
            var genericObject = new Object<T>();
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

    internal interface IObject
    {
        void SendTo(IReference receiver);
    }

    public interface IReference
    {
        void Accept<T>(T parameter);
    }

    internal class Object<T> : IObject
    {
        private T Value { get; set; }

        public void Set(T value)
        {
            Value = value;
        }

        public void SendTo(IReference receiver)
        {
            receiver.Accept(Value);
        }
    }
}