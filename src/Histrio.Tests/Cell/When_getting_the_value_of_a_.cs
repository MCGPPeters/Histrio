using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.Cell
{
    public abstract class When_getting_the_value_of_a_<T> : GivenSubject<Theater>
    {
        private Address _customer;
        private T _expectedValue;
        private Address _storageCell;
        private TaskCompletionSource<Reply<T>> _taskCompletionSource;

        protected When_getting_the_value_of_a_(T expectedValue)
        {
            Given(() =>
            {
                _expectedValue = expectedValue;

                _storageCell = Subject.CreateActor(new CellBehavior<T>());
                var set = new Set<T>(_expectedValue);
                Subject.Dispatch(set, _storageCell);

                _taskCompletionSource = new TaskCompletionSource<Reply<T>>();
                _customer = Subject.CreateActor(new AssertionBehavior<Reply<T>>(_taskCompletionSource, 1));
            });

            When(() =>
            {
                var get = new Get(_customer);
                Subject.Dispatch(get, _storageCell);
            });
        }

        [Fact]
        public async Task Then_value_of_the_retrieved_primitive_is_returned()
        {
            var actualValue = await _taskCompletionSource.Task;
            actualValue.Content.ShouldBeEquivalentTo(_expectedValue);
        }
    }
}