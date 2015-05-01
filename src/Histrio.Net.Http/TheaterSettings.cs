using System;

namespace Histrio.Net.Http
{
    /// <summary>
    ///     Settings for hosting theater middleware
    /// </summary>
    public class TheaterSettings
    {
        /// <summary>
        ///     Gets or sets the theater.
        /// </summary>
        /// <value>
        ///     The theater.
        /// </value>
        public Theater Theater { get; set; }

        /// <summary>
        /// Gets or sets the endpoint address.
        /// </summary>
        /// <value>
        /// The endpoint address.
        /// </value>
        public Uri EndpointAddress { get; set; }
    }
}