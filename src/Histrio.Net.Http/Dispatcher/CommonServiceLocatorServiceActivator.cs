using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace Histrio.Net.Http.Dispatcher
{
    public class CommonServiceLocatorServiceActivator : IHttpControllerActivator
    {
        public IHttpController Create(HttpRequestMessage request
            , HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = ServiceLocator.Current.GetInstance(controllerType) as IHttpController;
            return controller;
        }
    }
}