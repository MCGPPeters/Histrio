namespace Histrio.Tests.Bus
{
    public class Event
    {
        public string EventType { get; private set; }
        public byte[] Data { get; private set; }
        public byte[] Metadata { get; private set; }
        public long CreatedEpoch { get; private set; }

        public Event(string eventType, byte[] data, byte[] metadata, long createdEpoch)
        {
            this.EventType = eventType;
            this.Data = data;
            this.Metadata = metadata;
            this.CreatedEpoch = createdEpoch;
        }
    }
}