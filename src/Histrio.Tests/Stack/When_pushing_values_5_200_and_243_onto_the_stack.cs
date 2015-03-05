using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Histrio.Tests.Stack
{
    [TestClass]
    public class When_pushing_values_5_200_and_243_onto_the_stack : When_pushing_values_onto_the_stack
    {
        public When_pushing_values_5_200_and_243_onto_the_stack() : base(valuesToPush: new[] { 5, 200, 243 }, numberOfPops: 2, expectedValueRetrievedByPop: 200)
        {
        }
    }
}