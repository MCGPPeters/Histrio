using System;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.Factorial
{
    public class When_calculating_factorial_of_x : GivenWhenThen
    {
        [Theory, 
            InlineData(0, 1),
            InlineData(1, 1),
            InlineData(3, 6),
            InlineData(4, 24)]
        public void Then_factorial_of_x_is_calculated(int input, int expectedValue)
        {
            Theater theater = new Theater();
            Address customer = null;
            var promiseOfTheActualValue = new TaskCompletionSource<CalculatedFactorial>();

            Given(() =>
            {
                customer = theater.CreateActor(new AssertionBehavior<CalculatedFactorial>(promiseOfTheActualValue, 1));
            });

            When(() =>
            {
                var factorialCalculator = theater.CreateActor(new FactorialCalculationBehavior());
                var calculateFactorialFor = new CalculateFactorialFor(input, customer);
                theater.Dispatch(calculateFactorialFor, factorialCalculator);
            });

            Then(async () =>
            {
                var calculatedFactorial = await promiseOfTheActualValue.Task;
                calculatedFactorial.Result.Should().Be(expectedValue);
            });
        }
    }
}