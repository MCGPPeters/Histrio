using System.Threading.Tasks;
using Chill;
using FluentAssertions;
using Histrio.Behaviors;
using Histrio.Behaviors.StorageCell;
using Histrio.Commands;
using Histrio.Expressions;
using Xunit;

namespace Histrio.Tests.StorageCell
{
    public abstract class When_getting_the_value_of_a_<T> : GivenSubject<Theater>
    {
        private T _actualValue;
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

                _storageCell = New.Actor(new StorageCellBehavior<T>());
                var set = new Set<T>(_expectedValue);
                Send.Message(set).To(_storageCell);

                _taskCompletionSource = new TaskCompletionSource<Reply<T>>();
                _customer = New.Actor(new TaskCompletionBehavior<Reply<T>>(_taskCompletionSource, 1));
            });

            When(() =>
            {
                var get = new Get(_customer);
                Send.Message(get).To(_storageCell);
            });
        }

        [Fact]
        public async Task Then_value_of_the_retrieved_primitive_is_returned()
        {
            var actualValue = await _taskCompletionSource.Task;
            actualValue.Body.ShouldBeEquivalentTo(_expectedValue);
        }
    }

    //public static class MessageMonad
    //{
    //    public static Message<T> AsMessage<T>(this T body)
    //    {
    //        return new Message<T>(body);
    //    }

    //    public static Message<U> SelectMany<T, U>(this Message<T> Message, Func<T, Message<U>> function)
    //    {
    //        return function(Message.Body);
    //    }
    //}
}