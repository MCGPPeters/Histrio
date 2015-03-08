using System.Threading;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors;
using Xunit;

namespace Histrio.Tests.Factorial
{
    public abstract class When_calculating_factorial_of_x : GivenSubject<System>
    {
        private readonly int _expectedValue;
        private int _actualValue;
        private IAddress _addressOfTheCustomer;

        protected When_calculating_factorial_of_x(int expectedInput, int expectedValue)
        {
            _expectedValue = expectedValue;
            Given(() =>
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                _addressOfTheCustomer =
                    Subject.AddressOf(new CallBackBehavior<FactorialCalculated>(v => { _actualValue = v.Result; },
                        taskFactory));
            });

            When(() =>
            {
                var addressOfFactorial = Subject.AddressOf(new FactorialBehavior());
                addressOfFactorial.Receive(new CalculateFactorialFor(expectedInput, _addressOfTheCustomer));
            });
        }

        [Fact]
        public void Then_factorial_of_x_is_calculated()
        {
            while (_actualValue.Equals(default(int)))
            {
            }

            _actualValue.ShouldBeEquivalentTo(_expectedValue);
        }
    }
}