using System;
using System.Threading.Tasks;

using Histrio.Behaviors;
using Histrio.Tests.StorageCell;

namespace Histrio.Tests.Factorial
{
    public class TestBehavior : BehaviorBase, IHandle<FactorialCalculated>
    {
        private readonly Action<FactorialCalculated> _callback;

        public TestBehavior(Action<FactorialCalculated> callback)
        {
            _callback = callback;
        }

        public Task Accept(FactorialCalculated message)
        {
            _callback(message);
            return Task.FromResult(0);
        }
    }
}