using EMailService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMailService.Services
{
    public interface IEMailService
    {
        Task<string> SendEmailAsync(RequestModel model);
        Task SendMailing();
        Task<string> RegistrateNewSubscriberAsync(SubscriberModel model);
        Task<string> AddNewTemplateAsync(TemplateModel model);
    }
}
