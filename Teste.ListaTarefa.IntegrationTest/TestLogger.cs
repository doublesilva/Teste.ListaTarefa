

using Microsoft.Extensions.Logging;

namespace Teste.ListaTarefa.IntegrationTest
{
    public static class TestLogger
    {
        static TestLogger()
        {
            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            Logger = LoggerFactory.CreateLogger("TestLogger");
        }
        public static ILogger Logger { get; private set; }

        public static ILoggerFactory LoggerFactory { get; private set; }
    }
}
