using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Histrio.Behaviors;

namespace Histrio
{
    public sealed class Theater
    {
        private readonly IActorNamingService _actorNamingService;
        private readonly Dictionary<IAddress, Buffer> _localAddresses = new Dictionary<IAddress, Buffer>();
        private readonly List<IDispatcher> _remoteMessageDispatchers = new List<IDispatcher>();

        public Theater(IActorNamingService actorNamingService)
        {
            _actorNamingService = actorNamingService;
            Name = Guid.NewGuid().ToString();
            SetInstance(this);
        }

        public string Name { get; private set; }

        private static Theater _instance;

        public static Theater GetInstance()
        {
            return _instance ?? new Theater(new InMemoryNamingService());
        }

        private static void SetInstance(Theater value)
        {
            _instance = value;
        }

        public void Register(Address address, BehaviorBase behavior)
        {
            var buffer = new Buffer(new BlockingCollection<IMessage>());
            var arbiter = new MailboxArbiter(buffer);
            new Actor(behavior, address, arbiter);
            _localAddresses.Add(address, buffer);
        }

        public void Dispatch<T>(Message<T> message)
        {
            var address = message.Address;
            if(_localAddresses.ContainsKey(address))
            {
                var buffer = _localAddresses[address];
                buffer.Add(message);
            }
            else
            {
                var actorLocation = _actorNamingService.ResolveActorLocation(address.UniversalActorName);
                var capableDispatchers = SelectDispatcherForCustomerOfMessage(actorLocation);
                foreach (var dispatcher in capableDispatchers)
                {
                    dispatcher.Dispatch(message, actorLocation);
                }
            }
        }

        private IEnumerable<IDispatcher> SelectDispatcherForCustomerOfMessage(Uri universalActorLocation)
        {
            return from dispatcher in _remoteMessageDispatchers
                where dispatcher.CanDispathFor(universalActorLocation)
                select dispatcher;
        }

        public void AddDispatcher(IDispatcher dispatcher)
        {
            _remoteMessageDispatchers.Add(dispatcher);
        }
    }
}