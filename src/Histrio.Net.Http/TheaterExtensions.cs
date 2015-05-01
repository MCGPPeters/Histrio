using System;
using System.Net.Http;
using Owin;

namespace Histrio.Net.Http
{
    /// <summary>
    ///     Extensio method for the <see cref="Theater" /> class
    /// </summary>
    public static class TheaterExtensions
    {
        /// <summary>
        ///     Permits the dispatch of messages over HTTP.
        /// </summary>
        /// <param name="theater">The theater.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public static void PermitMessageDispatchOverHttp(this Theater theater, HttpClient httpClient)
        {
            theater.AddDispatcher(new HttpDispatcher(httpClient));
        }

        /// <summary>
        /// Adds the HTTP end point.
        /// </summary>
        /// <param name="theater">The theater.</param>
        /// <param name="endpointAddress">The endpoint address.</param>
        /// <param name="appBuilder">The application builder.</param>
        public static void AddHttpEndPoint(this Theater theater, Uri endpointAddress, IAppBuilder appBuilder)
        {
            var histrioSettings = new TheaterSettings
            {
                Theater = theater,
                EndpointAddress = endpointAddress
            };
            theater.AddEndpoint(endpointAddress);
            appBuilder.UseTheater(histrioSettings);
        }
    }
}