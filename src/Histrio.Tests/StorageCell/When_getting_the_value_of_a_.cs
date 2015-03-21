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
    public abstract class When_getting_the_value_of_a_<T> : GivenSubject<System>
    {
        private T _actualValue;
        private IAddress _customer;
        private T _expectedValue;
        private IAddress _storageCell;
        private TaskCompletionSource<T> _taskCompletionSource;

        protected When_getting_the_value_of_a_(T expectedValue)
        {
            Given(() =>
            {
                Context.System = Subject;
                _expectedValue = expectedValue;

                _storageCell = New.Actor(new StorageCellBehavior<T>());
                var set = new Set<T>(_expectedValue);
                Send.Message(set).To(_storageCell);

                _taskCompletionSource = new TaskCompletionSource<T>();
                _customer = New.Actor(new TaskCompletionBehavior<T>(_taskCompletionSource, 1));
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
            _actualValue = await _taskCompletionSource.Task;
            _actualValue.ShouldBeEquivalentTo(_expectedValue);
        }
    }

    //public static class MessageMonad
    //{
    //    public static IMessage<T> AsMessage<T>(this T body)
    //    {
    //        return new Message<T>(body);
    //    }

    //    public static IMessage<U> SelectMany<T, U>(this IMessage<T> message, Func<T, IMessage<U>> function)
    //    {
    //        return function(message.Body);
    //    }
    //}
}