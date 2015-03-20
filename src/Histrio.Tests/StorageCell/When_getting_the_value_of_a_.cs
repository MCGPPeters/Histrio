using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors;
using Histrio.Behaviors.StorageCell;
using Xunit;

namespace Histrio.Tests.StorageCell
{
    public abstract class When_getting_the_value_of_a_<T> : GivenSubject<System>
    {
        private T _actualValue;
        private IAddress _customer;
        private IAddress _storageCell;
        private T _expectedValue;
        private TaskCompletionSource<T> _taskCompletionSource;

        protected When_getting_the_value_of_a_(T expectedValue)
        {
            Given(() =>
            {
                Context.System = Subject;
                _expectedValue = expectedValue;

                _storageCell = New.Actor(new StorageCellBehavior<T>());
                var set = new Set<T>(_expectedValue);
                var message = New.Message(set).To(_storageCell);
                Send.Message(message);

                _taskCompletionSource = new TaskCompletionSource<T>();
                _customer = New.Actor(new TaskCompletionBehavior<T>(_taskCompletionSource, 1));
            });

            When(() =>
            {
                var get = new Get(_customer);
                var message = New.Message(get).To(_storageCell);
                Send.Message(message);
            });
        }

        [Fact]
        public async Task Then_value_of_the_retrieved_primitive_is_returned()
        {
            _actualValue = await _taskCompletionSource.Task;
            _actualValue.ShouldBeEquivalentTo(_expectedValue);
        }
    }
}