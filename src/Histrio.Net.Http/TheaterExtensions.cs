using System.Net.Http;

namespace Histrio.Net.Http
{
    public static class TheaterExtensions
    {
        public static void PermitMessageDispatchOverHttp(this Theater theater, HttpClient httpClient)
        {
            theater.AddDispatcher(new HttpDispatcher(httpClient));
        }
    }
}