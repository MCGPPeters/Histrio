using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Histrio.Net.Http
{
    public class HttpDispatcher : IDispatcher
    {
        private readonly HttpClient _httpClient;

        public HttpDispatcher(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool CanDispathFor(Uri universalActorLocation)
        {
            return universalActorLocation.Scheme == "http";
        }

        public async void Dispatch<T>(Message<T> message, Uri universalActorLocation)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            UntypedMessage untypedMessage = new UntypedMessage(typeof(T).AssemblyQualifiedName, message.Address.UniversalActorName.ToString(), message.Body);

            var response = await _httpClient.PostAsJsonAsync(universalActorLocation, untypedMessage);
            Debug.AutoFlush = true;


            var readAsStringAsync = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(readAsStringAsync);
        }
    }

    public class UntypedMessage
    {
        public string AssemblyQualifiedName { get; set; }
        public object Body { get; set; }
        public string Address { get; set; }

        public UntypedMessage(string assemblyQualifiedName, string address, object body)
        {
            AssemblyQualifiedName = assemblyQualifiedName;
            Body = body;
            Address = address;
        }
    }
}