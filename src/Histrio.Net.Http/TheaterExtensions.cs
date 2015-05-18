using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        /// <param name="httpMessageHandler"></param>
        /// <param name="baseAddress"></param>
        public static void PermitMessageDispatchOverHttp(this Theater theater, HttpMessageHandler httpMessageHandler, Uri baseAddress)
        {
            theater.AddDispatcher(new HttpDispatcher(httpMessageHandler, baseAddress));
        }

        /// <summary>
        ///     Permits dispatch over HTTP using th default HttpClientHandler
        /// </summary>
        /// <param name="theater"></param>
        /// <param name="baseAddress"></param>
        public static void PermitMessageDispatchOverHttp(this Theater theater, Uri baseAddress)
        {
            PermitMessageDispatchOverHttp(theater, new HttpClientHandler(), baseAddress);
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