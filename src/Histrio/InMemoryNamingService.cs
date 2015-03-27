using System;
using System.Collections.Generic;

namespace Histrio
{
    /// <summary>
    ///     An implementation of a naming service that stores locations of actor addresses in memory
    ///     Suitable for testing scenario's and static configuration of Actor systems that span multiple
    ///     locations
    /// </summary>
    public class InMemoryNamingService : IActorNamingService
    {
        private readonly Dictionary<Address, Uri> _addresses = new Dictionary<Address, Uri>();

        /// <summary>
        ///     Resolves the actor location.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public Uri ResolveActorLocation(Address address)
        {
            return _addresses[address];
        }

        /// <summary>
        ///     Registers the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="universalActorLocation">The location of the Actor</param>
        public void Register(Address address, Uri universalActorLocation)
        {
            _addresses.Add(address, universalActorLocation);
        }
    }
}