namespace Histrio
{
    internal class Envelope<T> : IEnvelope<T>
    {
        public Envelope(T body)
        {
            Body = body;
        }

        public void GetHandledBy(IHandle<T> behavior)
        {
            behavior.Accept(Body);
        }

        public T Body { get; private set; }
    }
}