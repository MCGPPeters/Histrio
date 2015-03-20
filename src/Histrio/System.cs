using System;
using System.Collections.Generic;
using Histrio.Behaviors;

namespace Histrio
{
    public sealed class System
    {
        public System(string name)
        {
            Name = name;
        }

        public System() : this(Guid.NewGuid().ToString())
        {
            
        }

        private readonly Dictionary<IAddress, Actor> _actorAddresses = new Dictionary<IAddress, Actor>();

        public string Name { get; private set; }

        public void Register(Address address, BehaviorBase behavior)
        {
            _actorAddresses.Add(address, new Actor(behavior, address));
        }

        public void Dispatch<T>(IMessage<T> message)
        {
            _actorAddresses[message.Address].Accept(message);
        }
    }
}