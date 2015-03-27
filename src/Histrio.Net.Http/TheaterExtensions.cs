using System.Net.Http;

namespace Histrio.Net.Http
{
    /// <summary>
    /// Extensio method for the <see cref="Theater"/> class
    /// </summary>
    public static class TheaterExtensions
    {
        /// <summary>
        /// Permits the dispatch of messages over HTTP.
        /// </summary>
        /// <param name="theater">The theater.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public static void PermitMessageDispatchOverHttp(this Theater theater, HttpClient httpClient)
        {
            theater.AddDispatcher(new HttpDispatcher(httpClient));
        }
    }
}