using System.Threading.Tasks;

using Histrio.Behaviors;
using Histrio.Tests.StorageCell;

namespace Histrio.Tests.Factorial
{
    public class FactorialContinuationBehavior : BehaviorBase, IHandle<CalculateFactorialFor>, IHandle<FactorialCalculated>
    {
        private readonly IAddress customer;
        private readonly int x;

        public FactorialContinuationBehavior(CalculateFactorialFor message)
        {
            x = message.X;
            customer = message.Customer;
        }

        public void Accept(CalculateFactorialFor message)
        {
            customer.Receive(new FactorialCalculated
            {
                For = message.X,
                Result = x * message.X
            });
        }

        public void Accept(FactorialCalculated message)
        {
            customer.Receive(new FactorialCalculated
            {
                For = message.For,
                Result = x * message.Result
            });
        }
    }
}