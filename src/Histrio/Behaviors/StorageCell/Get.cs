namespace Histrio.Behaviors.StorageCell
{
    public class Get
    {
        public Get(IAddress address)
        {
            Costumer = address;
        }

        public IAddress Costumer { get; set; }
    }
}