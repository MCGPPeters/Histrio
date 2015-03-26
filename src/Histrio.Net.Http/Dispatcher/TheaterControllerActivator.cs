using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Histrio.Net.Http.Controlers;

namespace Histrio.Net.Http.Dispatcher
{
    public class TheaterControllerActivator : IHttpControllerActivator
    {
        private readonly Theater _theater;

        public TheaterControllerActivator(Theater theater)
        {
            _theater = theater;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            return new TheaterController(_theater);
        }
    }
}