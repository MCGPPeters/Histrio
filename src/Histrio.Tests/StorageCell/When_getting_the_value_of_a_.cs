using Chill;
using FluentAssertions;

using Histrio.Behaviors;
using Histrio.Behaviors.StorageCell;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.StorageCell
{
    public abstract class When_getting_the_value_of_a_<T> : GivenSubject<System>
    {
        private T _actualValue;
        private IAddress _addressOfTheCustomer;
        private IAddress _addressOfThePrimitive;
        private T _expectedValue;

        protected When_getting_the_value_of_a_(T expectedValue)
        {
            Given(() =>
            {
                _expectedValue = expectedValue;
                _addressOfThePrimitive = Subject.AddressOf(new StorageCellBehavior<T>());
                _addressOfThePrimitive.Receive(new Set<T>(_expectedValue));
                _addressOfTheCustomer = Subject.AddressOf(new TestBehavior<T>(v => _actualValue = v));
            });

            When(() => _addressOfThePrimitive.Receive(new Get(_addressOfTheCustomer)));
        }

        [TestMethod]
        public void Then_value_of_the_retrieved_primitive_is_returned()
        {
            while (_actualValue.Equals(default(T)))
            {
            }
            _actualValue.ShouldBeEquivalentTo(_expectedValue);
        }
    }
}