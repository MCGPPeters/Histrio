using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors;
using Histrio.Collections.Stack;
using Xunit;

namespace Histrio.Tests.Stack
{
    public abstract class When_pushing_values_onto_the_stack : GivenSubject<System>
    {
        private readonly int _numberOfPops;
        private readonly int _expectedValueRetrievedByPop;
        private IAddress _customer;
        private IAddress _stack;
        private readonly TaskCompletionSource<int> _promiseOfTheActualValue = new TaskCompletionSource<int>();

        protected When_pushing_values_onto_the_stack(IEnumerable<int> valuesToPush, int numberOfPops,
            int expectedValueRetrievedByPop)
        {
            _numberOfPops = numberOfPops;
            _expectedValueRetrievedByPop = expectedValueRetrievedByPop;
            Given(() =>
            {
                _stack = New.Actor(new StackBehavior<int>(default(int), null));
                foreach (var i in valuesToPush)
                {
                    var push = new Push<int>(i);
                    var message = New.Message(push).To(_stack);
                    Send.Message(message);
                }
                _customer = New.Actor(new TaskCompletionBehavior<int>(_promiseOfTheActualValue, _numberOfPops));
            });

            When(() =>
            {
                for (var i = 0; i < numberOfPops; i++)
                {
                    var pop = new Pop(_customer);
                    var message = New.Message(pop).To(_stack);
                    Send.Message(message);
                }
            });
        }

        [Fact]
        public async Task Then_the_value_on_top_of_the_stack_is_retrieved_by_a_pop()
        {
           var  actualValue = await _promiseOfTheActualValue.Task;

            actualValue.Should().Be(_expectedValueRetrievedByPop);
        }
    }
}