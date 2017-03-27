using System.Threading;
using System.Threading.Tasks;

namespace Histrio.Tests
{
    /// <summary>
    ///     A behavior used for performing assertions on messages. Because it is indeterminate when
    ///     a message will arrive, for testing purposes it is desirable to have a way of getting
    ///     notified when a message does arrive. This behavior does that using a taskcompletion source. The pattern used is
    ///     that
    ///     one sets the actor hosting this behavior as the ultimate customer of an interaction between actors that needs to be
    ///     tested.
    ///     Than in the await the taskcompletion to get the message that was delivered to the actor hosting this behavior and
    ///     perform the assertions
    ///     on the result of the task (representing the message)
    /// </summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    public class AssertionBehavior<TMessage> : IBehavior
    {
        private readonly int _numberOfExpectedMessagesWhereAfterToComplete;
        private readonly TaskCompletionSource<TMessage> _taskCompletionSource;
        private int _numberOfMessagesReceived;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssertionBehavior{TMessage}" /> class.
        /// </summary>
        /// <param name="taskCompletionSource">The task completion source </param>
        /// <param name="numberOfExpectedMessagesWhereAfterToComplete">The number of expected messages where after to complete.</param>
        public AssertionBehavior(TaskCompletionSource<TMessage> taskCompletionSource,
            int numberOfExpectedMessagesWhereAfterToComplete)
        {
            _taskCompletionSource = taskCompletionSource;
            _numberOfExpectedMessagesWhereAfterToComplete = numberOfExpectedMessagesWhereAfterToComplete;
        }

        /// <summary>
        ///     Accepts the specified message. Accept is Actor Model terminology for "I can do something with this message"
        /// </summary>
        /// <param name="message">The message.</param>
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