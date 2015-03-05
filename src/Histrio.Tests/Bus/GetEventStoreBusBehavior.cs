using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.Core;
using EventStore.Core.Data;
using Newtonsoft.Json;
using ExpectedVersion = EventStore.ClientAPI.ExpectedVersion;

namespace Histrio.Tests.Bus
{
    public class GetEventStoreBusBehavior : BusBehaviorBase, IDisposable
    {
        private readonly IEventStoreConnection eventStoreConnection;
        private readonly ClusterVNode _node;
        private readonly Task<bool> _eventStoreInitialized;

        public GetEventStoreBusBehavior()
        {
            var source = new TaskCompletionSource<bool>();
            _eventStoreInitialized = source.Task;
            var notListening = new IPEndPoint(IPAddress.None, 0);
            _node = EmbeddedVNodeBuilder
                .AsSingleNode()
                .WithExternalTcpOn(notListening)
                .WithInternalTcpOn(notListening)
                .WithExternalHttpOn(notListening)
                .WithInternalHttpOn(notListening)
                .RunInMemory()
                .RunProjections(ProjectionsMode.All);
            _node.NodeStatusChanged += (_, e) =>
            {
                if (e.NewVNodeState != VNodeState.Master)
                {
                    return;
                }
                source.SetResult(true);
            };
            eventStoreConnection = EmbeddedEventStoreConnection.Create(_node);
            eventStoreConnection.ErrorOccurred += (sender, args) =>
            {
                Debug.WriteLine(args.Exception.Message);
            };

            eventStoreConnection.Disconnected += (sender, args) =>
            {
                Debug.WriteLine(args.Connection);
            };

            _node.Start();
        }

        public override async Task Accept(Publish message)
        {
            await _eventStoreInitialized;
            var serializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };

            var @event = message.Event;
            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event, serializerSettings));

            var eventHeaders = new Dictionary<string, object>()
            {
                {
                    "EventClrTypeName", @event.GetType().AssemblyQualifiedName
                }
            };
            var metadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventHeaders, serializerSettings));
            var typeName = @event.GetType().AssemblyQualifiedName;

            var writeResult = await eventStoreConnection.AppendToStreamAsync(message.Topic.Name, ExpectedVersion.Any,
                new EventData(Guid.NewGuid(), typeName, true, data, metadata));
        }

        public override async Task Accept(Subscribe message)
        {
            await _eventStoreInitialized;

            var subscription1 = eventStoreConnection.SubscribeToStreamFrom(message.Topic.Name, null, false,
                (subscription, @event) =>
                {
                    var originalEvent = @event.OriginalEvent;
                    var type = Type.GetType(originalEvent.EventType);
                    
                    var typedMessage = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(originalEvent.Data), type);
                    message.Subscriber.Receive(typedMessage);
                });
        }

        public void Dispose()
        {
            _node.Stop();
            eventStoreConnection.Dispose();
        }
    }
}