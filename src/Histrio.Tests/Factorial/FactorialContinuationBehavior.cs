using Histrio.Behaviors;

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
            _customer.Receive(new FactorialCalculated
            {
                For = message.X,
                Result = _x*message.X
            });
        }

        public void Accept(FactorialCalculated message)
        {
            _customer.Receive(new FactorialCalculated
            {
                For = message.For,
                Result = _x*message.Result
            });
        }
    }
}