namespace Histrio
{
    /// <summary>
    /// A message to send to the <see cref="CellBehavior{T}"/> to retrieve content from it
    /// </summary>
    public class Get
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Get"/> class.
        /// </summary>
        /// <param name="customer">The customer receiving the content</param>
        public Get(Address customer)
        {
            Customer = customer;
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <value>
        /// The customer receiving the content from the Actor hosting the <see cref="CellBehavior{T}"/>
        /// </value>
        public Address Customer { get; private set; }
    }
}