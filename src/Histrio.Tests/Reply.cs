namespace Histrio.Tests
{
    internal class Reply<T>
    {
        public Reply(T value)
        {
            Value = value;
        }
        public T Value { get; }
    }
}