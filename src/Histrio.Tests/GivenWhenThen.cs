using System;
using Serilog;
using Serilog.Events;

namespace Histrio.Tests
{
    public class GivenWhenThen : global::Chill.GivenWhenThen
    {
        protected GivenWhenThen()
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(LogEventLevel.Verbose)
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger = logger;
        }

        public void Then(Action action)
        {
            action();
        }
    }
}