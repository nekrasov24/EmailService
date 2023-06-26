using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EMailService.Services
{
    [DisallowConcurrentExecution]
    public class SchedulerService : IJob
    {
        private readonly IEMailService _emailService;
        private readonly ILogger _logger;

        public SchedulerService(IEMailService emailService, ILogger logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _emailService.SendMailing();
        }
    }
}
