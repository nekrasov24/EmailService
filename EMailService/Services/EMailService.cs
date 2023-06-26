using EMailService.Models;
using EMailService.Repository;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EMailService.Services
{
    public class EMailService : IEMailService
    {
        private readonly EmailSettingsModel _emailSettingsModel;
        private readonly IRepository<Subscriber> _subscriberRepository;
        private readonly IRepository<EmailTemplate> _templateRepository;
        private readonly ILogger _logger;


        public EMailService(EmailSettingsModel emailSettingsModel, IRepository<Subscriber> subscriberRepository,
            IRepository<EmailTemplate> templateRepository, ILogger logger)
        {
            _emailSettingsModel = emailSettingsModel;
            _subscriberRepository = subscriberRepository;
            _templateRepository = templateRepository;
            _logger = logger;
        }

        public async Task<string> SendEmailAsync(RequestModel requestModel)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_emailSettingsModel.Email);
                email.To.Add(MailboxAddress.Parse(requestModel.EMailAdrress));
                email.Subject = requestModel.Topic;
                var builder = new BodyBuilder();
                if (requestModel.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in requestModel.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = requestModel.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettingsModel.Host, _emailSettingsModel.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettingsModel.Email, _emailSettingsModel.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                _logger.Information($"Mail was sent to {requestModel.MailOwner}");
                return $"Mail was sent to {requestModel.MailOwner}";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendMailing()
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_emailSettingsModel.Email);
                var subscribers = await _subscriberRepository.GetAllAsync();
                var getTemplate = await _templateRepository.FindAsync();

                using var smtp = new SmtpClient();
                smtp.Connect(_emailSettingsModel.Host, _emailSettingsModel.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailSettingsModel.Email, _emailSettingsModel.Password);
                foreach (var subscriber in subscribers)
                {
                    email.To.Add(MailboxAddress.Parse(subscriber.Email));
                    email.Subject = getTemplate.Topic;
                    var builder = new BodyBuilder();
                    builder.HtmlBody = getTemplate.Template;
                    email.Body = builder.ToMessageBody();
                    await smtp.SendAsync(email);
                    _logger.Information($"Mail was sent to {subscriber.Name} as a mailing");
                }
                smtp.Disconnect(true);
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<string> AddNewTemplateAsync(TemplateModel model)
        {
            try
            {
                var template = await _templateRepository.GetAllAsync(t => t.Template.Equals(model.Template));

                if (template != null) throw new Exception("Template is already exists");
                var newTemplate = new EmailTemplate()
                {
                    Id = Guid.NewGuid(),
                    Template = model.Template,
                    Topic = model.Topic
                };

                await _templateRepository.AddAsync(newTemplate);
                _logger.Information("New template was added");
                return "New template was added";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> RegistrateNewSubscriberAsync(SubscriberModel model)
        {
            try
            {
                var template = await _subscriberRepository.GetAllAsync(t => t.Email.Equals(model.Email));

                if (template != null) throw new Exception("Template is already exists");
                var newSubscriber = new Subscriber()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Email = model.Email
                };

                await _subscriberRepository.AddAsync(newSubscriber);
                _logger.Information("New subscriber was added");
                return "New subscriber was added";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
