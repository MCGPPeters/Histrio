using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Factorial
{
    [TestClass]
    public class When_calculating_factorial_of_4 : When_calculating_factorial_of_x
    {
        public When_calculating_factorial_of_4() : base(expectedInput: 4, expectedValue: 24)
        {
        }
    }
}