namespace Histrio.Behaviors.StorageCell
{
    public class Set<T>
    {
        public Set(T body)
        {
            Body = body;
        }

        public T Body { get; set; }
    }
}