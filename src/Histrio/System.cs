using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Histrio.Behaviors;

namespace Histrio
{
    public sealed class System
    {
        private readonly Dictionary<IAddress, Buffer> _actorAddresses = new Dictionary<IAddress, Buffer>();

        public System()
        {
            Name = Guid.NewGuid().ToString();
        }

        public string Name { get; private set; }

        public void Register(Address address, BehaviorBase behavior)
        {
            var buffer = new Buffer(new BlockingCollection<IMessage>());
            var arbiter = new MailboxArbiter(buffer);
            new Actor(behavior, address, arbiter);
            _actorAddresses.Add(address, buffer);
        }

        public void Dispatch<T>(Message<T> message)
        {
            var buffer = _actorAddresses[message.Address];
            buffer.Add(message);
        }
    }
}