using Histrio.Behaviors;

namespace Histrio.Tests.Factorial
{
    public class FactorialCalculationBehavior : BehaviorBase, IHandle<CalculateFactorialFor>
    {
        public void Accept(CalculateFactorialFor message)
        {
            Actor.Become(New.Actor(new FactorialCalculationBehavior()));

            var x = message.X;
            if (x == 0)
            {
                var factorialCalculated = new FactorialCalculated {For = x, Result = 1};
                var factorialCalculatedMessage = New.Message(factorialCalculated).To(message.Customer);
                Send.Message(factorialCalculatedMessage);
            }
            else
            {
                var continuation = New.Actor(new FactorialContinuationBehavior(message));
                var calculateFactorialFor = New.Message(new CalculateFactorialFor(x - 1, continuation)).To(Actor.Address);
                Send.Message(calculateFactorialFor);
            }
        }
    }
}