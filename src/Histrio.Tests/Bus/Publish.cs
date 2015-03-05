namespace Histrio.Tests.Bus
{
    public class Publish
    {
        private readonly object @event;
        private readonly Topic _topic;

        public Publish(object @event, Topic topic)
        {
            this.@event = @event;
            _topic = topic;
        }

        public object Event
        {
            get { return @event; }
        }

        public Topic Topic
        {
            get { return _topic; }
        }

        public string Type { get; set; }
    }
}