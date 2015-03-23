using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Histrio.Net.Http.Dispatcher
{
    public class HttpControllerTypeResolver<TController> : IHttpControllerTypeResolver
    {
        public ICollection<Type> GetControllerTypes(IAssembliesResolver _)
        {
            var controllerTypes = typeof(TController)
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IHttpController).IsAssignableFrom(t))
                .ToList();
            return controllerTypes;
        }
    }
}