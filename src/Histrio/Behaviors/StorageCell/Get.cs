namespace Histrio.Behaviors.StorageCell
{
    public class Get
    {
        public Get(Address address)
        {
            Customer = address;
        }

        public Address Customer { get; private set; }
    }
}