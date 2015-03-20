namespace Histrio
{
    public static class Context
    {
        static Context()
        {
            System = new System();
        }

        public static System System { get; set; }
    }
}