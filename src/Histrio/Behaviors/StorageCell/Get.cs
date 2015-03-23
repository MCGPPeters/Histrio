namespace Histrio.Behaviors.StorageCell
{
    public class Get
    {
        public Get(IAddress address)
        {
            Customer = address;
        }

        public IAddress Customer { get; private set; }
    }
}