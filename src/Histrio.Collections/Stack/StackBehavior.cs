using Histrio.Behaviors;

namespace Histrio.Collections.Stack
{
    public class StackBehavior<T> : BehaviorBase, IHandle<Push<T>>, IHandle<Pop>
    {
        private readonly T _content;
        private readonly IAddress _link;

        public StackBehavior(T content, IAddress link)
        {
           _content = content;
           _link = link;
        }

        public void Accept(Pop message)
        {
            Actor.Become(_link);
            var contentMessage = New.Message(_content).To(message.Customer);
            Send.Message(contentMessage);
        }

        public void Accept(Push<T> message)
        {
            var p = New.Actor(new StackBehavior<T>(_content, _link));
            Actor.Become(New.Actor(new StackBehavior<T>(message.Value, p)));
        }
    }
}

