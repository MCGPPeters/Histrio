using Histrio.Behaviors;
using Histrio.Commands;
using Histrio.Expressions;

namespace Histrio.Collections.Stack
{
    public class StackNodeBehavior<T> : BehaviorBase, IHandle<Push<T>>, IHandle<Pop>
    {
        private readonly T _content;
        private readonly IAddress _link;

        public StackNodeBehavior(T content, IAddress link)
        {
            _content = content;
            _link = link;
        }

        public void Accept(Pop message)
        {
            Actor.Become(_link);
            Send.Message(_content).To(message.Customer);
        }

        public void Accept(Push<T> message)
        {
            var p = New.Actor(new StackNodeBehavior<T>(_content, _link));
            var stackNodBehaviore = new StackNodeBehavior<T>(message.Value, p);
            var stackNode = New.Actor(stackNodBehaviore);
            Actor.Become(stackNode);
        }
    }
}