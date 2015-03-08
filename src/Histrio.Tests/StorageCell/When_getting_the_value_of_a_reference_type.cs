namespace Histrio.Tests.StorageCell
{
    public abstract class When_getting_the_value_of_a_reference_type<T> : When_getting_the_value_of_a_<T>
        where T : class
    {
        protected When_getting_the_value_of_a_reference_type(T expectedValue) : base(expectedValue)
        {
        }
    }
}