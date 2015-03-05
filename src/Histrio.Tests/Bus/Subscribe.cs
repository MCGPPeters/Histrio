namespace Histrio.Tests.Bus
{
    public class Subscribe
    {
        private readonly IAddress _subscriber;
        private readonly Topic _topic;

        public Topic Topic
        {
            get { return _topic; }
        }

        public IAddress Subscriber
        {
            get { return _subscriber; }
        }

        public Subscribe(Topic topic, IAddress subscriber)
        {
            _topic = topic;
            _subscriber = subscriber;
        }
    }
}