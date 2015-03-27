using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Histrio.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
    internal class HttpDispatcher : IDispatcher
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDispatcher"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        internal HttpDispatcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Determines whether this instance [can dispatch to] the specified actor location.
        /// </summary>
        /// <param name="actorLocation">The actor location.</param>
        /// <returns></returns>
        public bool CanDispatchTo(Uri actorLocation)
        {
            return actorLocation.Scheme == "http";
        }

        /// <summary>
        /// Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="actorLocation">The actor location.</param>
        public async void Dispatch<T>(Message<T> message, Uri actorLocation)
        {
            var untypedMessage = new UntypedMessage(typeof(T).AssemblyQualifiedName,
                message.To.ActorName, message.Body);

            await _httpClient.PostAsJsonAsync(actorLocation, untypedMessage);

            //TODO : => exception handling based on the statuscode, response.StatusCode;
        }
    }
}