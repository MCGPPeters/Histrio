using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Histrio.Logging;

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
        private readonly List<Uri> _endpointAddresses = new List<Uri>();

        private static readonly ILog Logger = LogProvider.For<Theater>();

        /// <summary>
        /// Adds the endpoint.
        /// </summary>
        /// <param name="endpointAddress">The endpoint address.</param>
        public void AddEndpoint(Uri endpointAddress)
        {
            EndpointAddresses.Add(endpointAddress);
        }



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
        /// Gets the endpoint addresses that can be used to communicate to this theater remotely
        /// </summary>
        /// <value>
        /// The endpoint addresses.
        /// </value>
        public List<Uri> EndpointAddresses
        {
            get
            {
                return _endpointAddresses;
            }
        }


        /// <summary>
        ///     Creates the actor.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        /// <returns></returns>
        public Address CreateActor(Behavior behavior)
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
        public Address CreateActor(Behavior behavior, string actorName)
        {
            var mailBox = new MailBox(new BlockingCollection<IMessage>());
            var address = Actor.Create(behavior, actorName, mailBox, this);
            _localAddresses.Add(address, mailBox);
            foreach (Uri endpointAddress in EndpointAddresses)
            {
                _actorNamingService.Register(address, endpointAddress);
            }
           
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

                Logger.DebugFormat("A message of type '{0}' was added to the mailbox of an actor in this theater at address '{1}'",
                    typeof(T), message.To);
            }
            else
            {
                var actorLocation = _actorNamingService.ResolveActorLocation(address);
                var capableDispatchers = SelectDispatchersForCustomerOfMessage(actorLocation);
                foreach (var dispatcher in capableDispatchers)
                {
                    dispatcher.Dispatch(message, actorLocation);

                    Logger.DebugFormat("A message of type '{0}' was dispatched to address '{1}' in a theater at location '{2}'",
                        typeof(T), message.To, actorLocation);
                }
            }

            Logger.TraceFormat("Message contents : {@message}", message.Body);
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