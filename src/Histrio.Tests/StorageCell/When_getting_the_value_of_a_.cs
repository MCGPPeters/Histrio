using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors.StorageCell;
using Histrio.Testing;
using Xunit;

namespace Histrio.Tests.StorageCell
{
    public abstract class When_getting_the_value_of_a_<T> : GivenSubject<Theater>
    {
        private IAddress _customer;
        private T _expectedValue;
        private IAddress _storageCell;
        private TaskCompletionSource<Reply<T>> _taskCompletionSource;

        protected When_getting_the_value_of_a_(T expectedValue)
        {
            SetThe<IActorNamingService>().To(new InMemoryNamingService());

            Given(() =>
            {
                _expectedValue = expectedValue;

                _storageCell = Subject.CreateActor(new StorageCellBehavior<T>());
                var set = new Set<T>(_expectedValue);
                var setMessage = set.AsMessage();
                setMessage.To = _storageCell;
                Subject.Dispatch(setMessage);

                _taskCompletionSource = new TaskCompletionSource<Reply<T>>();
                _customer = Subject.CreateActor(new AssertionBehavior<Reply<T>>(_taskCompletionSource, 1));
            });

            When(() =>
            {
                var get = new Get(_customer);
                var getMessage = get.AsMessage();
                getMessage.To = _storageCell;
                Subject.Dispatch(getMessage);
            });
        }

        [Fact]
        public async Task Then_value_of_the_retrieved_primitive_is_returned()
        {
            var actualValue = await _taskCompletionSource.Task;
            actualValue.Body.ShouldBeEquivalentTo(_expectedValue);
        }
    }
}