using Histrio.Behaviors;
using Histrio.Commands;
using Histrio.Expressions;

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
                Send.Message(factorialCalculated).To(message.Customer);
            }
            else
            {
                var continuation = New.Actor(new FactorialContinuationBehavior(message));
                var calculateFactorialFor = new CalculateFactorialFor(x - 1, continuation);
                Send.Message(calculateFactorialFor).To(Actor.Address);
            }
        }
    }
}