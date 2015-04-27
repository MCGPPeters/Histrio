using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Histrio.Collections.Stack;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.Stack
{
    public class When_pushing_values_onto_the_stack : GivenWhenThen
    {
        [Theory,
         InlineData(new[] {1, 2, 3}, 1, 3),
         InlineData(new[] { 5, 200, 243 }, 2, 200)]
        public void Then_a_specific_value_is_retrieved_by_a_number_of_pops(int[] valuesToPush,
            int numberOfPops,
            int expectedValueRetrievedByPop)
        {
            Theater theater = new Theater();
            var promiseOfTheActualValue = new TaskCompletionSource<int>();
            Address customer = null;
            Address stack = null;

            Given(() =>
            {
                SetThe<IActorNamingService>().To(new InMemoryActorNamingService());

                stack = theater.CreateActor(new StackNodeBehavior<int>(default(int), null));
                foreach (var i in valuesToPush)
                {
                    var push = new Push<int>(i);
                    theater.Dispatch(push, stack);
                }
                customer = theater.CreateActor(new AssertionBehavior<int>(promiseOfTheActualValue, numberOfPops));
            });

            When(() =>
            {
                for (var i = 0; i < numberOfPops; i++)
                {
                    var pop = new Pop(customer);
                    theater.Dispatch(pop, stack);
                }
            });

            Then(async () =>
            {
                var actualValue = await promiseOfTheActualValue.Task;
                actualValue.Should().Be(expectedValueRetrievedByPop);
            });
        }
    }
}