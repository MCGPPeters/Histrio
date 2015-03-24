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
            var factorialCalculated = new FactorialCalculated
            {
                For = message.X,
                Result = _x*message.X
            };
            SendFactorialCalculated(factorialCalculated);
        }

        public void Accept(FactorialCalculated message)
        {
            var factorialCalculated = new FactorialCalculated
            {
                For = message.For,
                Result = _x*message.Result
            };
            SendFactorialCalculated(factorialCalculated);
        }

        private void SendFactorialCalculated(FactorialCalculated factorialCalculated)
        {
            var factorialCalculatedMessage = factorialCalculated.AsMessage();
            factorialCalculatedMessage.To = _customer;
            Actor.Send(factorialCalculatedMessage);
        }
    }
}