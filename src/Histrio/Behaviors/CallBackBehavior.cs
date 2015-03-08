using System;
using System.Threading.Tasks;

namespace Histrio.Behaviors
{
    public class CallBackBehavior<TMessage> : BehaviorBase, IHandle<TMessage>
    {
        private readonly Action<TMessage> _callback;
        private readonly TaskFactory _taskFactory;

        public CallBackBehavior(Action<TMessage> callback, TaskFactory taskFactory)
        {
            _callback = callback;
            _taskFactory = taskFactory;
        }

        public async void Accept(TMessage message)
        {
            await _taskFactory.StartNew(() => _callback(message));
        }
    }
}