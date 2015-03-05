namespace Histrio.Behaviors.StorageCell
{
    public class Reply<T>
    {
        public Reply(T body)
        {
            Body = body;
        }

        public T Body { get; private set; }
    }
}