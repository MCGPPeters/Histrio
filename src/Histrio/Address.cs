using System;

namespace Histrio
{
    public class Address : IAddress
    {
        public Address(Uri universalActorName, Theater theater)
        {
            Theater = theater;
            UniversalActorName = universalActorName;
        }

        public Uri UniversalActorName { get; }

        public Theater Theater { get; private set; }
    }
}