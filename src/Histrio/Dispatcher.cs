namespace Histrio
{
    internal class Dispatcher : IAccept
    {
        private readonly System _system;

        public Dispatcher(System system, Address address)
        {
            _system = system;
        }

        public void Accept<T>(T content)
        {
          
        }
    }
}