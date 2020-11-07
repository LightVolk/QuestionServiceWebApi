using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Infrastructure.LoggerInfrastructure
{
    public class EfCoreLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogEventLevel logEventLevel=LogEventLevel.Information;
            switch (logLevel)
            {
                case LogLevel.Critical:
                    logEventLevel = LogEventLevel.Fatal;
                    break;
                case LogLevel.Debug:
                    logEventLevel = LogEventLevel.Debug;
                    break;
                case LogLevel.Error:
                    logEventLevel = LogEventLevel.Error;
                    break;
                case LogLevel.Information:
                    logEventLevel = LogEventLevel.Information;
                    break;
                case LogLevel.None:
                    break;
                case LogLevel.Trace:
                    logEventLevel = LogEventLevel.Verbose;
                    break;
                case LogLevel.Warning:
                    logEventLevel = LogEventLevel.Warning;
                    break;
            }

            if (logLevel==LogLevel.None)
            {
                return;
            }

            Serilog.Log.Logger.Write(logEventLevel,exception, formatter(state, exception));
        }
    }
}
