using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace QuestionServiceWebApi.Infrastructure.LoggerInfrastructure
{
    public class EfCoreLoggerProvider : ILoggerProvider
    {
        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new EfCoreLogger();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
