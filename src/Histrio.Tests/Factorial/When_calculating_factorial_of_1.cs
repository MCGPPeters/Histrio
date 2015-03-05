using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Factorial
{
    [TestClass]
    public class When_calculating_factorial_of_1 : When_calculating_factorial_of_x
    {
        public When_calculating_factorial_of_1() : base(expectedInput: 1, expectedValue: 1)
        {
        }
    }
}