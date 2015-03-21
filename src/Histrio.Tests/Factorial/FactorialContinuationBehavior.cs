using Histrio.Behaviors;
using Histrio.Commands;

namespace Histrio.Tests.Factorial
{
    public class FactorialContinuationBehavior : BehaviorBase, IHandle<CalculateFactorialFor>,
        IHandle<FactorialCalculated>
    {
        private readonly IAddress _customer;
        private readonly int _x;

        public FactorialContinuationBehavior(CalculateFactorialFor message)
        {
            _x = message.X;
            _customer = message.Customer;
        }

        public void Accept(CalculateFactorialFor message)
        {
            var factorialCalculated = new FactorialCalculated
            {
                For = message.X,
                Result = _x*message.X
            };
            Send.Message(factorialCalculated).To(_customer);
        }

        public void Accept(FactorialCalculated message)
        {
            var factorialCalculated = new FactorialCalculated
            {
                For = message.For,
                Result = _x*message.Result
            };
            Send.Message(factorialCalculated).To(_customer);
        }
    }
}