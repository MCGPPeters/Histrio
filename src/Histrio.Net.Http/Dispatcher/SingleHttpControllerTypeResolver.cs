using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Histrio.Net.Http.Dispatcher
{
    /// <summary>
    /// Returns a single type of controller based on the type argument
    /// More efficient then the default that is convention based... and scans the assembly for controllers
    /// </summary>
    /// <typeparam name="TController">The type of the controller.</typeparam>
    public class SingleHttpControllerTypeResolver<TController> : IHttpControllerTypeResolver where TController : ApiController
    {
        /// <summary>
        /// Gets the controller types.
        /// </summary>
        /// <param name="_">The _.</param>
        /// <returns></returns>
        public ICollection<Type> GetControllerTypes(IAssembliesResolver _)
        {
            return new[] { typeof (TController)};
        }
    }
}