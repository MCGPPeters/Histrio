using System;
using Histrio.Behaviors;

namespace Histrio.Tests.Factorial
{
    public class TestBehavior : BehaviorBase, IHandle<FactorialCalculated>
    {
        private readonly Action<FactorialCalculated> _callback;

        public TestBehavior(Action<FactorialCalculated> callback)
        {
            _callback = callback;
        }

        public void Accept(FactorialCalculated message)
        {
            _callback(message);
        }
    }
}