using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.StorageCell
{
    [TestClass]
    public class When_getting_the_value_of_a_integer : When_getting_the_value_of_a_value_type<int>
    {
        public When_getting_the_value_of_a_integer() : base(333)
        {
        }
    }
}