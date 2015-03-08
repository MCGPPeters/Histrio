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
        private int _actualValue;
        private IAddress _addressOfTheCustomer;
        private IAddress _addressOfTheStack;
        private int _popsReceived;

        protected When_pushing_values_onto_the_stack(IEnumerable<int> valuesToPush, int numberOfPops,
            int expectedValueRetrievedByPop)
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            _numberOfPops = numberOfPops;
            _expectedValueRetrievedByPop = expectedValueRetrievedByPop;
            Given(() =>
            {
                _addressOfTheStack = Subject.AddressOf(new StackBehavior<int>(default(int), null));
                foreach (var i in valuesToPush)
                {
                    _addressOfTheStack.Receive(new Push<int>(i));
                }
                _addressOfTheCustomer = Subject.AddressOf(new CallBackBehavior<int>(v =>
                {
                    _popsReceived++;
                    _actualValue = v;
                }, taskFactory));
            });

            When(() =>
            {
                for (var i = 0; i < numberOfPops; i++)
                {
                    _addressOfTheStack.Receive(new Pop(_addressOfTheCustomer));
                }
            });
        }

        [Fact]
        public void Then_the_value_on_top_of_the_stack_is_retrieved_by_a_pop()
        {
            while (_actualValue.Equals(default(int)) || _popsReceived != _numberOfPops)
            {
            }

            _actualValue.ShouldBeEquivalentTo(_expectedValueRetrievedByPop);
        }
    }
}