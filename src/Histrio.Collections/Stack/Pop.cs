namespace Histrio.Collections.Stack
{
    public class Pop
    {
        public Pop(Address customer)
        {
            Customer = customer;
        }

        public Address Customer { get; private set; }
    }
}