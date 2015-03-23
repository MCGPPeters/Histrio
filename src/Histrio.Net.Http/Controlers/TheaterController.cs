using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Histrio.Behaviors;
using Histrio.Commands;
using Newtonsoft.Json;

namespace Histrio.Net.Http.Controlers
{
    internal class TheaterController : ApiController
    {
        //private readonly Theater _theater;

        //public TheaterController(Theater theater)
        //{
        //    _theater = theater;
        //}

        [Route("")]
        public HttpResponseMessage Post([FromBody] UntypedMessage untypedMessage)
        {
            //var content = await Request.Content.ReadAsByteArrayAsync();
            //var untypedMessage = ((UntypedMessage)
            //        JsonConvert.DeserializeObject(Encoding.UTF8.GetString(content), typeof(UntypedMessage)));
            var conversionType = Type.GetType(untypedMessage.AssemblyQualifiedName);
            var jtoken = untypedMessage.Body;
            var messageBody = JsonConvert.DeserializeObject(jtoken.ToString(), conversionType);
            //var messageBody = Convert.ChangeType(body, conversionType);
            var universalActorName = new Uri(untypedMessage.Address);
            var address = new Address(universalActorName, null);
            Send.Message(messageBody).To(address);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}