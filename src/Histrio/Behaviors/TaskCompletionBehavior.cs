using System;
using System.Threading;
using System.Threading.Tasks;

namespace Histrio.Behaviors
{
    public class TaskCompletionBehavior<TMessage> : BehaviorBase, IHandle<TMessage>
    {
        private readonly TaskCompletionSource<TMessage> _taskCompletionSource;
        private readonly int _numberOfExpectedMessagesWhereAfterToComplete;
        private int _numberOfMessagesReceived;

        public TaskCompletionBehavior(TaskCompletionSource<TMessage> taskCompletionSource, int numberOfExpectedMessagesWhereAfterToComplete)
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