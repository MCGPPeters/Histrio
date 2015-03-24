using System.Threading;
using System.Threading.Tasks;
using Histrio.Behaviors;

namespace Histrio.Testing
{
    public class AssertionBehavior<TMessage> : BehaviorBase, IHandle<TMessage>
    {
        private readonly int _numberOfExpectedMessagesWhereAfterToComplete;
        private readonly TaskCompletionSource<TMessage> _taskCompletionSource;
        private int _numberOfMessagesReceived;

        public AssertionBehavior(TaskCompletionSource<TMessage> taskCompletionSource,
            int numberOfExpectedMessagesWhereAfterToComplete)
        {
            _taskCompletionSource = taskCompletionSource;
            _numberOfExpectedMessagesWhereAfterToComplete = numberOfExpectedMessagesWhereAfterToComplete;
        }

        public void Accept(TMessage message)
        {
            Interlocked.Increment(ref _numberOfMessagesReceived);
            if (_numberOfMessagesReceived == _numberOfExpectedMessagesWhereAfterToComplete)
            {
                _taskCompletionSource.SetResult(message);
            }
        }
    }
}