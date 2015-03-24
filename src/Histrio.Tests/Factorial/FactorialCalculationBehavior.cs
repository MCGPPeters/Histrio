using Histrio.Behaviors;
using Histrio.Commands;
using Histrio.Expressions;

namespace Histrio.Tests.Factorial
{
    public class FactorialCalculationBehavior : BehaviorBase, IHandle<CalculateFactorialFor>
    {
        public void Accept(CalculateFactorialFor message)
        {
            var address = Actor.Create(new FactorialCalculationBehavior());
            Actor.Become(address);

            var x = message.X;
            if (x == 0)
            {
                var factorialCalculated = new FactorialCalculated {For = x, Result = 1};
                var factorialCalculatedMessage = factorialCalculated.AsMessage();
                factorialCalculatedMessage.To = message.Customer;
                Actor.Send(factorialCalculatedMessage);
            }
            else
            {
                var continuation = Actor.Create(new FactorialContinuationBehavior(message));
                var calculateFactorialFor = new CalculateFactorialFor(x - 1, continuation);
                var calculateFactorialForMessage = calculateFactorialFor.AsMessage();
                calculateFactorialForMessage.To = Actor.Address;
                Actor.Send(calculateFactorialForMessage);
            }
        }
    }
}