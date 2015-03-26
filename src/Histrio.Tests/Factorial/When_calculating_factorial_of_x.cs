using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.Factorial
{
    public abstract class When_calculating_factorial_of_x : GivenSubject<Theater>
    {
        private readonly int _expectedValue;

        private readonly TaskCompletionSource<FactorialCalculated> _promiseOfTheActualValue =
            new TaskCompletionSource<FactorialCalculated>();

        private Address _customer;

        protected When_calculating_factorial_of_x(int expectedInput, int expectedValue)
        {
            _expectedValue = expectedValue;
            Given(() =>
            {
                SetThe<IActorNamingService>().To(new InMemoryNamingService());

                _customer = Subject.CreateActor(new AssertionBehavior<FactorialCalculated>(_promiseOfTheActualValue, 1));
            });

            When(() =>
            {
                var factorialCalculator = Subject.CreateActor(new FactorialCalculationBehavior());
                var calculateFactorialFor = new CalculateFactorialFor(expectedInput, _customer);
                var calculateFactorialForMessage = calculateFactorialFor.AsMessage();
                calculateFactorialForMessage.To = factorialCalculator;
                Subject.Dispatch(calculateFactorialForMessage);
            });
        }

        [Fact]
        public async Task Then_factorial_of_x_is_calculated()
        {
            var actualValue = await _promiseOfTheActualValue.Task;

            actualValue.Result.Should().Be(_expectedValue);
        }
    }
}