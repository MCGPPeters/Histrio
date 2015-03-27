namespace Histrio.Collections.Stack
{
    /// <summary>
    ///     A behavior implementing a strongly type stack
    /// </summary>
    /// <typeparam name="T">The type of the value stored into a stack node</typeparam>
    public class StackNodeBehavior<T> : BehaviorBase, IHandle<Push<T>>, IHandle<Pop>
    {
        private readonly T _content;
        private readonly Address _link;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StackNodeBehavior{T}" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="link">The link.</param>
        public StackNodeBehavior(T content, Address link)
        {
            _content = content;
            _link = link;
        }

        /// <summary>
        ///     Accepts the specified message. Accept is Actor Model terminology for "I can do something with this message"
        /// </summary>
        /// <param name="message">The message.</param>
        public void Accept(Pop message)
        {
            Actor.Become(_link);
            Actor.Send(_content, message.Customer);
        }

        /// <summary>
        ///     Accepts the specified message. Accept is Actor Model terminology for "I can do something with this message"
        /// </summary>
        /// <param name="message">The message.</param>
        public void Accept(Push<T> message)
        {
            var p = Actor.Create(new StackNodeBehavior<T>(_content, _link));
            var stackNodeBehavior = new StackNodeBehavior<T>(message.Value, p);
            var stackNode = Actor.Create(stackNodeBehavior);
            Actor.Become(stackNode);
        }
    }
}