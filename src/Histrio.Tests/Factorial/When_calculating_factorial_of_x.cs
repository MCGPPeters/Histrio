using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors;
using Histrio.Commands;
using Histrio.Expressions;
using Xunit;

namespace Histrio.Tests.Factorial
{
    public abstract class When_calculating_factorial_of_x : GivenSubject<System>
    {
        private readonly int _expectedValue;

        private readonly TaskCompletionSource<FactorialCalculated> _promiseOfTheActualValue =
            new TaskCompletionSource<FactorialCalculated>();

        private IAddress _customer;

        protected When_calculating_factorial_of_x(int expectedInput, int expectedValue)
        {
            _expectedValue = expectedValue;
            Given(() =>
            {
                Context.System = Subject;

                _customer = New.Actor(new TaskCompletionBehavior<FactorialCalculated>(_promiseOfTheActualValue, 1));
            });

            When(() =>
            {
                var factorialCalculator = New.Actor(new FactorialCalculationBehavior());
                var calculateFactorialFor = new CalculateFactorialFor(expectedInput, _customer);
                Send.Message(calculateFactorialFor).To(factorialCalculator);
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