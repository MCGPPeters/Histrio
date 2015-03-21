using System;

namespace Histrio
{
    public class Address : IAddress
    {
        public Address(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}