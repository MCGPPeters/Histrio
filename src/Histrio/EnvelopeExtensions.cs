namespace Histrio
{
    internal static class EnvelopeExtensions
    {
        public static IEnvelope<T> InEnvelope<T>(this T body)
        {
            return new Envelope<T>(body);
        }
    }
}