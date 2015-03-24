using Histrio.Behaviors;

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
            var content = _content.AsMessage();
            content.To = message.Customer;
            Actor.Send(content);
        }

        public void Accept(Push<T> message)
        {
            var p = Actor.Create(new StackNodeBehavior<T>(_content, _link));
            var stackNodeBehavior = new StackNodeBehavior<T>(message.Value, p);
            var stackNode = Actor.Create(stackNodeBehavior);
            Actor.Become(stackNode);
        }
    }
}