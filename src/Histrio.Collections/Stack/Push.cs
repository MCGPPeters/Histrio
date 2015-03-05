namespace Histrio.Collections.Stack
{
    public class Push<T>
    {
        public Push(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}