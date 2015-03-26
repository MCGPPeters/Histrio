namespace Histrio.Net.Http
{
    public class UntypedMessage
    {
        public UntypedMessage(string assemblyQualifiedName, string address, object body)
        {
            AssemblyQualifiedName = assemblyQualifiedName;
            Body = body;
            Address = address;
        }

        public string AssemblyQualifiedName { get; set; }
        public object Body { get; set; }
        public string Address { get; set; }
    }
}