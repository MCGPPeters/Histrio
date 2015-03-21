using System;
using System.Collections.Concurrent;
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

        private readonly Dictionary<IAddress, Buffer> _actorAddresses = new Dictionary<IAddress, Buffer>();
        private readonly List<Actor> actors = new List<Actor>();

        public string Name { get; private set; }

        public void Register(Address address, BehaviorBase behavior)
        {
            
            var buffer = new Buffer(new BlockingCollection<IMessage>());
            var arbiter = new MailboxArbiter(buffer);
            var actor = new Actor(behavior, address, arbiter);
            actors.Add(actor);
            _actorAddresses.Add(address, buffer);
        }

        public void Dispatch<T>(Message<T> message)
        {
            var buffer = _actorAddresses[message.Address];
            buffer.Add(message);
        }
    }
}