using System.Threading.Tasks;

namespace Histrio.Behaviors
{
    public abstract class BehaviorBase
    {
        protected internal IActor Actor { get; set; }
        protected internal System System { get; set; }

        public async Task Accept<TMessage>(IMessage<TMessage> message)
        {
            var handler = this as IHandle<TMessage>;
            if (ThisBehaviorHandlesThisTypeOfMessages(handler))
            {
                await message.GetHandledBy(handler);
            }
            else
            {
                await AcceptCore(message);
            }
        }

        protected virtual Task AcceptCore<T>(IMessage<T> message)
        {
            return Task.FromResult(false);
        }

        private static bool ThisBehaviorHandlesThisTypeOfMessages<TMessage>(IHandle<TMessage> handler)
        {
            return null != handler;
        }
    }
}