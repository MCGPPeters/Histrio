using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Histrio.Net.Http.Logging;

namespace Histrio.Net.Http
{
    /// <summary>
    /// </summary>
    internal class HttpDispatcher : IDispatcher
    {
        private static readonly ILog Logger = LogProvider.For<HttpDispatcher>();

        private readonly HttpClient _httpClient;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HttpDispatcher" /> class.
        /// </summary>
        /// <param name="httpMessageHandler"></param>
        /// <param name="baseAddress"></param>
        internal HttpDispatcher(HttpMessageHandler httpMessageHandler, Uri baseAddress)
        {
            _httpClient = new HttpClient(httpMessageHandler) { BaseAddress = baseAddress};
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        ///     Determines whether this instance [can dispatch to] the specified actor location.
        /// </summary>
        /// <param name="actorLocation">The actor location.</param>
        /// <returns></returns>
        public bool CanDispatchTo(Uri actorLocation)
        {
            return actorLocation.Scheme == "http";
        }

        /// <summary>
        ///     Dispatches the specified message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message.</param>
        /// <param name="actorLocation">The actor location.</param>
        public async void Dispatch<T>(Message<T> message, Uri actorLocation)
        {
            var untypedMessage = new UntypedMessage(message.Body.GetType().AssemblyQualifiedName,
                message.To.ActorName, message.Body);

            var response = await _httpClient.PostAsJsonAsync(actorLocation, untypedMessage).ConfigureAwait(false);

            Logger.DebugFormat("Sent message of type '{0}' via http to address '{1}' hosted in a theater at location '{2}'",
                typeof(T), message.To, actorLocation);

            Logger.TraceFormat("Sent message contents : {@message}", untypedMessage);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Logger.ErrorFormat("Failed to send the message '{@message}' to the location {0}. Reason : {1} {2}", message, actorLocation, response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}