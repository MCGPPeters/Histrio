using System;

namespace Histrio
{
    /// <summary>
    /// An Actor naming service is used to resolve the location (represented by a Uri) of an Actor.
    /// Think of it it as DNS for addresses that reference Actors
    /// </summary>
    public interface IActorNamingService
    {
        /// <summary>
        /// Resolves the actor location.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        Uri ResolveActorLocation(Address address);

        /// <summary>
        /// Registers the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="universalActorLocation">The location of the Actor</param>
        void Register(Address address, Uri universalActorLocation);
    }
}