namespace Histrio.Tests.Bus.Transformation
{
    public class Transform<TSource>
    {
        public IAddress AddressOfTheCustomer { get; }
        public TSource Source { get; }

        public Transform(TSource _source, IAddress _addressOfTheCustomer)
        {
            Source = _source;
            AddressOfTheCustomer = _addressOfTheCustomer;
        }
    }
}