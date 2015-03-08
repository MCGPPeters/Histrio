namespace Histrio
{
    internal class Cell<T> : ICell
    {
        private T Value { get; set; }

        public void SendValueTo(IAccept receiver)
        {
            receiver.Accept(Value);
        }

        public void Set(T value)
        {
            Value = value;
        }
    }
}