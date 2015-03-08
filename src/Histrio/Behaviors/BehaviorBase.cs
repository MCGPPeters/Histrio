using System.Threading.Tasks;

namespace Histrio.Behaviors
{
    public abstract class BehaviorBase
    {
        protected internal IActor Actor { get; set; }
        protected internal System System { get; set; }

        internal void Accept<TMessage>(IEnvelope<TMessage> envelope)
        {
            var handler = this as IHandle<TMessage>;
            if (ThisBehaviorHandlesThisTypeOfMessages(handler))
            {
                envelope.GetHandledBy(handler);
            }
            else
            {
                AcceptCore(envelope);
            }
        }

        protected internal virtual void AcceptCore<T>(IEnvelope<T> envelope) { }

        private static bool ThisBehaviorHandlesThisTypeOfMessages<TMessage>(IHandle<TMessage> handler)
        {
            return null != handler;
        }
    }
}