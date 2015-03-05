namespace Histrio.Tests.Bus
{
    public class Topic
    {
        private readonly string _name;

        public Topic(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}