using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Stack
{
    [TestClass]
    public class When_pushing_values_1_2_and_3_onto_the_stack : When_pushing_values_onto_the_stack
    {
        public When_pushing_values_1_2_and_3_onto_the_stack() : base(valuesToPush: new[] { 1, 2, 3 }, numberOfPops: 1, expectedValueRetrievedByPop: 3)
        {
        }
    }
}