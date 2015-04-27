namespace Histrio.Tests.Factorial
{
    public class FactorialContinuationBehavior : BehaviorBase, IHandle<CalculateFactorialFor>,
        IHandle<CalculatedFactorial>
    {
        private readonly Address _customer;
        private readonly int _x;

        public FactorialContinuationBehavior(CalculateFactorialFor message)
        {
            _x = message.X;
            _customer = message.Customer;
        }

        public void Accept(CalculateFactorialFor message)
        {
            var factorialCalculated = new CalculatedFactorial
            {
                For = message.X,
                Result = _x*message.X
            };
            SendFactorialCalculated(factorialCalculated);
        }

        public void Accept(CalculatedFactorial message)
        {
            var factorialCalculated = new CalculatedFactorial
            {
                For = message.For,
                Result = _x*message.Result
            };
            SendFactorialCalculated(factorialCalculated);
        }

        private void SendFactorialCalculated(CalculatedFactorial factorialCalculated)
        {
            Actor.Send(factorialCalculated, _customer);
        }
    }
}