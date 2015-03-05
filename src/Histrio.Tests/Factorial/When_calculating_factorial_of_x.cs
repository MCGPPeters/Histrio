using Chill;

using FluentAssertions;

using Histrio.Tests.StorageCell;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Factorial
{
    public abstract class When_calculating_factorial_of_x : GivenSubject<System>
    {
        private readonly int _expectedValue;
        private int _actualValue;
        private IAddress _addressOfFactorial;
        private IAddress _addressOfTheCustomer;

        protected When_calculating_factorial_of_x(int expectedInput, int expectedValue)
        {
            _expectedValue = expectedValue;
            Given(() =>
            {
                _addressOfFactorial = Subject.AddressOf(new FactorialBehavior());
                _addressOfTheCustomer = Subject.AddressOf(new TestBehavior(v =>
                {
                    _actualValue = v.Result;
                }));
            });

            When(() => _addressOfFactorial.Receive(new CalculateFactorialFor(expectedInput, _addressOfTheCustomer)));
        }

        [TestMethod]
        public void Then_factorial_of_x_is_calculated()
        {
            while (_actualValue.Equals(default(int)))
            {
            }
            _actualValue.ShouldBeEquivalentTo(_expectedValue);
        }
    }
}