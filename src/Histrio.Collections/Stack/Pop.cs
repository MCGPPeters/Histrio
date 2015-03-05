namespace Histrio.Collections.Stack
{
    public class Pop
    {
        public Pop(IAddress customer)
        {
            Customer = customer;
        }

        public IAddress Customer { get; private set; }
    }
}