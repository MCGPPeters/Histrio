using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Histrio
{
    /// <summary>
    ///     A Theater is a isolated container of Actors running at a location (server / vm / ...)
    ///     Multiple Theaters can exist at a location
    /// </summary>
    public sealed class Theater
    {
        private readonly IActorNamingService _actorNamingService;
        private readonly Dictionary<Address, MailBox> _localAddresses = new Dictionary<Address, MailBox>();
        private readonly List<IDispatcher> _remoteMessageDispatchers = new List<IDispatcher>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="Theater" /> class.
        /// </summary>
        /// <param name="actorNamingService">The actor naming service.</param>
        public Theater(IActorNamingService actorNamingService)
        {
            _actorNamingService = actorNamingService;
            Name = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Creates a theater with an <see cref="InMemoryActorNamingService" /> as the default <see cref="IActorNamingService" />
        /// </summary>
        /// <returns></returns>
        public Theater() : this(new InMemoryActorNamingService())
        {
            
        }

        private string Name { get; set; }

        /// <summary>
        ///     Creates the actor.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public Address CreateActor(BehaviorBase behavior)
        {
            var universalActorName = string.Format("uan://{0}/{1}", Name, Guid.NewGuid());
            return CreateActor(behavior, universalActorName);
        }

        /// <summary>
        ///     Creates the actor.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <param name="actorName">Name of the actor.</param>
        /// <returns></returns>
        public Address CreateActor(BehaviorBase behavior, string actorName)
        {
            var mailBox = new MailBox(new BlockingCollection<IMessage>());
            var address = Actor.Create(behavior, actorName, mailBox, this);
            _localAddresses.Add(address, mailBox);
            return address;
        }

        /// <summary>
        ///     Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageContent">The content that will be wtapped in a message.</param>
        /// <param name="actorName">Name of the universal actor.</param>
        public void Dispatch<T>(T messageContent, string actorName)
        {
            Dispatch(messageContent.AsMessage(), new Address(actorName));
        }

        /// <summary>
        ///     Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageContent">The content that will be wtapped in a message.</param>
        /// <param name="to">Address to send the message to</param>
        public void Dispatch<T>(T messageContent, Address to)
        {
            Dispatch(messageContent.AsMessage(), to);
        }

        /// <summary>
        ///     Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="to">Address to send the message to</param>
        public void Dispatch<T>(Message<T> message, Address to)
        {
            message.To = to;
            Dispatch(message);
        }

        /// <summary>
        ///     Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
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
                var actorLocation = _actorNamingService.ResolveActorLocation(address);
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
                where dispatcher.CanDispatchTo(universalActorLocation)
                select dispatcher;
        }

        /// <summary>
        ///     Adds the dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public void AddDispatcher(IDispatcher dispatcher)
        {
            _remoteMessageDispatchers.Add(dispatcher);
        }
    }
}