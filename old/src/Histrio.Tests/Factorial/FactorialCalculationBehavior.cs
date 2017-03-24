namespace Histrio.Tests.Factorial
{
    public class FactorialCalculationBehavior : Behavior, IHandle<CalculateFactorialFor>
    {
        public void Accept(CalculateFactorialFor message)
        {
            var address = Actor.Create(new FactorialCalculationBehavior());
            Actor.Become(address);

            var x = message.X;
            if (x == 0)
            {
                var factorialCalculated = new CalculatedFactorial{For = x, Result = 1};
                Actor.Send(factorialCalculated, message.Customer);
            }
            else
            {
                var continuation = Actor.Create(new FactorialContinuationBehavior(message));
                var calculateFactorialFor = new CalculateFactorialFor(x - 1, continuation);
                Actor.Send(calculateFactorialFor, Actor.Address);
            }
        }
    }
}