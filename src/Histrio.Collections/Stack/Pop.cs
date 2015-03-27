namespace Histrio.Collections.Stack
{
    /// <summary>
    /// A message to send to a StackNode (<see cref="StackNodeBehavior{T}"/>) to pop a value from it
    /// </summary>
    public class Pop
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pop"/> class.
        /// </summary>
        /// <param name="customer">The customer that will receive the value stored in the <see cref="StackNodeBehavior{T}"/></param>
        public Pop(Address customer)
        {
            Customer = customer;
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        public Address Customer { get; private set; }
    }
}