
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMailService.Services
{
    public class LogService
    {
        private readonly ILogger _logger;

        public LogService(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteDown(string mes)
        {
            _logger.Information(mes);
        }
    }
}

