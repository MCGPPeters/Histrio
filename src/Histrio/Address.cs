using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Histrio
{
    public class Address : IAddress
    {
        public Uri Uri { get; private set; }
        public Address(Uri uri)
        {
            Uri = uri;
        }
    }
}