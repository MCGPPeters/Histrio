using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Factorial
{
    [TestClass]
    public class When_calculating_factorial_of_0 : When_calculating_factorial_of_x
    {
        public When_calculating_factorial_of_0() : base(expectedInput: 0, expectedValue: 1)
        {
        }
    }
}