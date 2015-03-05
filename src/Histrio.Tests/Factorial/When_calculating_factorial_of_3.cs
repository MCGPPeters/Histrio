using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Factorial
{
    [TestClass]
    public class When_calculating_factorial_of_3 : When_calculating_factorial_of_x
    {
        public When_calculating_factorial_of_3() : base(expectedInput: 3, expectedValue: 6)
        {
        }
    }
}