using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.StorageCell
{
    [TestClass]
    public class When_getting_the_value_of_a_boolean : When_getting_the_value_of_a_value_type<bool>
    {
        public When_getting_the_value_of_a_boolean() : base(true)
        {
        }
    }
}