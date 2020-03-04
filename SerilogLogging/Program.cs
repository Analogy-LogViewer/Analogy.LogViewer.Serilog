using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace SerilogLogging
{
    public class Program
    {
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("log.txt",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            Log.Information("Hello, Serilog!");

            Log.CloseAndFlush();
        }
    }
}
