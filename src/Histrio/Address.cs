namespace Histrio
{
    /// <summary>
    ///     Represents a reference to an Actor.
    /// </summary>
    public class Address
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Address" /> class.
        /// </summary>
        /// <param name="actorName">Name of the actor. An actor name should be a unique reference to a specific Actor at runtime</param>
        /// 
        public Address(string actorName)
        {
            // For deserialization purposes, keep this contructor public
            ActorName = actorName;
        }

        /// <summary>
        ///     Gets the name of the actor.
        /// </summary>
        /// <value>
        ///     The name of the actor.
        /// </value>
        public string ActorName { get; private set; }

        private bool Equals(Address other)
        {
            return Equals(ActorName, other.ActorName);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (ActorName != null ? ActorName.GetHashCode() : 0);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals((Address) obj);
        }
    }
}