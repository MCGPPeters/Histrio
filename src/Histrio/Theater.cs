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
        private readonly Dictionary<IAddress, MailBox> _localAddresses = new Dictionary<IAddress, MailBox>();
        private readonly List<IDispatcher> _remoteMessageDispatchers = new List<IDispatcher>();

        public Theater(IActorNamingService actorNamingService)
        {
            _actorNamingService = actorNamingService;
            Name = Guid.NewGuid().ToString();
        }

        private string Name { get; set; }

        public IAddress CreateActor(BehaviorBase behavior)
        {
            var uriString = string.Format("uan://{0}/{1}", Name, Guid.NewGuid());
            var universalActorName = new Uri(uriString);
            var address = new Address(universalActorName);
            var mailBox = new MailBox(new BlockingCollection<IMessage>());
            _localAddresses.Add(address, mailBox);
            new Actor(behavior, address, mailBox, this);
            return address;
        }

        public void Dispatch<T>(Message<T> message, Uri universalActorName)
        {
            message.To = new Address(universalActorName);
            Dispatch(message);
        }

        public void Dispatch<T>(Message<T> message)
        {
            var address = message.To;
            if (_localAddresses.ContainsKey(address))
            {
                var buffer = _localAddresses[address];
                buffer.Add(message);
            }
            else
            {
                var actorLocation = _actorNamingService.ResolveActorLocation(address.UniversalActorName);
                var capableDispatchers = SelectDispatchersForCustomerOfMessage(actorLocation);
                foreach (var dispatcher in capableDispatchers)
                {
                    dispatcher.Dispatch(message, actorLocation);
                }
            }
        }

        private IEnumerable<IDispatcher> SelectDispatchersForCustomerOfMessage(Uri universalActorLocation)
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