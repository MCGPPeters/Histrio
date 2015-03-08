using System;
using System.Threading.Tasks;

using Histrio.Behaviors;
using Histrio.Behaviors.StorageCell;

namespace Histrio.Tests.StorageCell
{
    public class TestBehavior<T> : BehaviorBase, IHandle<Reply<T>>
    {
        private readonly Action<T> _callback;

        public TestBehavior(Action<T> callback)
        {
            _callback = callback;
        }

        public void Accept(Reply<T> message)
        {
            _callback(message.Body);
        }
    }
}