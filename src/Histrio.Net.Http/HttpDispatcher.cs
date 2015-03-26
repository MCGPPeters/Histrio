using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Histrio.Net.Http
{
    public class HttpDispatcher : IDispatcher
    {
        private readonly HttpClient _httpClient;

        public HttpDispatcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool CanDispathFor(Uri universalActorLocation)
        {
            return universalActorLocation.Scheme == "http";
        }

        public async void Dispatch<T>(Message<T> message, Uri universalActorLocation)
        {
            var untypedMessage = new UntypedMessage(typeof (T).AssemblyQualifiedName,
                message.To.ActorName.ToString(), message.Body);

            var response = await _httpClient.PostAsJsonAsync(universalActorLocation, untypedMessage);
            Debug.AutoFlush = true;


            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(readAsStringAsync);
        }
    }
}