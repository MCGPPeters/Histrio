using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.StorageCell
{
    [TestClass]
    public class When_getting_the_value_of_a_reference_type : When_getting_the_value_of_a_reference_type<bool[]>
    {
        private static readonly bool[] Values = { true, true };

        public When_getting_the_value_of_a_reference_type()
            : base(Values)
        {
        }
    }

    [TestClass]
    public abstract class When_getting_the_value_of_a_value_type<T> : When_getting_the_value_of_a_<T> where T : struct
    {
        protected When_getting_the_value_of_a_value_type(T expectedValue) : base(expectedValue)
        {
        }
    }
}