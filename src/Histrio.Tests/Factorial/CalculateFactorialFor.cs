namespace Histrio.Tests.Factorial
{
    public class CalculateFactorialFor
    {
        public CalculateFactorialFor(int x, IAddress customer)
        {
            X = x;
            Customer = customer;
        }

        public int X { get; private set; }
        public IAddress Customer { get; private set; }
    }
}