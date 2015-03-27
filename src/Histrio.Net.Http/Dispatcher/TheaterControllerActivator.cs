using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Histrio.Net.Http.Controlers;

namespace Histrio.Net.Http.Dispatcher
{
    /// <summary>
    ///     We only have 1 controller, namely the this one. Just create an instance
    /// </summary>
    public class TheaterControllerActivator : IHttpControllerActivator
    {
        private readonly Theater _theater;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TheaterControllerActivator" /> class.
        /// </summary>
        /// <param name="theater">The theater.</param>
        public TheaterControllerActivator(Theater theater)
        {
            _theater = theater;
        }

        /// <summary>
        ///     Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <returns></returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            return new TheaterController(_theater);
        }
    }
}