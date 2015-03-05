using System.Threading.Tasks;

using Histrio.Behaviors;
using Histrio.Tests.StorageCell;

namespace Histrio.Tests.Factorial
{
    public class FactorialBehavior : BehaviorBase, IHandle<CalculateFactorialFor>
    {
        public async Task Accept(CalculateFactorialFor message)
        {
            Actor.Become(System.AddressOf(new FactorialBehavior()));

            var x = message.X;
            if (x == 0)
            {
                await message.Customer.Receive(new FactorialCalculated { For = x, Result = 1 });
            }
            else
            {
                var continuation = System.AddressOf(new FactorialContinuationBehavior(message));
                await Actor.Address.Receive(new CalculateFactorialFor(x - 1, continuation));
            }
        }
    }
}