using System;

namespace Histrio
{
    public class Address : IAddress
    {
        private bool Equals(IAddress other)
        {
            return Equals(UniversalActorName, other.UniversalActorName);
        }

        public override int GetHashCode()
        {
            return (UniversalActorName != null ? UniversalActorName.GetHashCode() : 0);
        }

        public Address(Uri universalActorName)
        {
            UniversalActorName = universalActorName;
        }

        public Uri UniversalActorName { get; }
        public override bool Equals(object obj)
        {
            return Equals((Address) obj);
        }
    }
}