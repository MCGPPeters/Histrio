using System;

namespace Histrio.Behaviors
{
    public abstract class BehaviorBase : IHandle
    {
        protected internal IActor Actor { get; set; }

        public virtual void Accept<T>(Message<T> message)
        {
            var handler = this as IHandle<T>;
            if (handler != null)
            {
                message.GetHandledBy(handler);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}