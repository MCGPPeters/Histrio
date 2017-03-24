namespace Histrio.Tests.Factorial
{
    public class CalculateFactorialFor
    {
        public CalculateFactorialFor(int x, Address customer)
        {
            X = x;
            Customer = customer;
        }

        public int X { get; private set; }
        public Address Customer { get; private set; }
    }
}