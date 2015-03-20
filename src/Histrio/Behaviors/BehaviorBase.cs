namespace Histrio.Behaviors
{
    public abstract class BehaviorBase
    {
        protected internal IActor Actor { get; set; }

        internal void Accept<TMessage>(IMessage<TMessage> message)
        {
            var handler = this as IHandle<TMessage>;
            if (ThisBehaviorHandlesThisTypeOfMessages(handler))
            {
                message.GetHandledBy(handler);
            }
            else
            {
                AcceptCore(message);
            }
        }

        protected virtual void AcceptCore<T>(IMessage<T> message)
        {
        }

        private static bool ThisBehaviorHandlesThisTypeOfMessages<TMessage>(IHandle<TMessage> handler)
        {
            return null != handler;
        }
    }
}