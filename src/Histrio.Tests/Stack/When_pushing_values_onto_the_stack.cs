using System.Collections.Generic;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Collections.Stack;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.Stack
{
    public abstract class When_pushing_values_onto_the_stack : GivenSubject<Theater>
    {
        private readonly int _expectedValueRetrievedByPop;
        private readonly int _numberOfPops;
        private readonly TaskCompletionSource<int> _promiseOfTheActualValue = new TaskCompletionSource<int>();
        private Address _customer;
        private Address _stack;

        protected When_pushing_values_onto_the_stack(IEnumerable<int> valuesToPush, int numberOfPops,
            int expectedValueRetrievedByPop)
        {
            _numberOfPops = numberOfPops;
            _expectedValueRetrievedByPop = expectedValueRetrievedByPop;

            Given(() =>
            {
                SetThe<IActorNamingService>().To(new InMemoryNamingService());

                _stack = Subject.CreateActor(new StackNodeBehavior<int>(default(int), null));
                foreach (var i in valuesToPush)
                {
                    var push = new Push<int>(i);
                    var pushMessage = push.AsMessage();
                    pushMessage.To = _stack;
                    Subject.Dispatch(pushMessage);
                }
                _customer = Subject.CreateActor(new AssertionBehavior<int>(_promiseOfTheActualValue, _numberOfPops));
            });

            When(() =>
            {
                for (var i = 0; i < numberOfPops; i++)
                {
                    var pop = new Pop(_customer);
                    var popMessage = pop.AsMessage();
                    popMessage.To = _stack;
                    Subject.Dispatch(popMessage);
                }
            });
        }

        [Fact]
        public async Task Then_the_value_on_top_of_the_stack_is_retrieved_by_a_pop()
        {
            var actualValue = await _promiseOfTheActualValue.Task;

            actualValue.Should().Be(_expectedValueRetrievedByPop);
        }
    }
}