namespace Histrio
{
    /// <summary>
    /// Represents a message that is used for enabling serialization of strongly typed messages (<see cref="Message{T}"/>) 
    /// and deserialing it afterwards
    /// </summary>
    public class UntypedMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UntypedMessage"/> class.
        /// </summary>
        /// <param name="assemblyQualifiedName">Name of the assembly qualified.</param>
        /// <param name="address">The address.</param>
        /// <param name="body">The body.</param>
        public UntypedMessage(string assemblyQualifiedName, string address, object body)
        {
            AssemblyQualifiedName = assemblyQualifiedName;
            Body = body;
            Address = address;
        }

        /// <summary>
        /// Gets or sets the name of the assembly qualified.
        /// </summary>
        /// <value>
        /// The name of the assembly qualified.
        /// </value>
        public string AssemblyQualifiedName { get; private set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public object Body { get; private set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; private set; }
    }
}