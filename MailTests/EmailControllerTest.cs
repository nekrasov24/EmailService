using EMailService.Controllers;
using EMailService.Models;
using EMailService.Repository;
using EMailService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using Xunit;

namespace MailTests
{
    public class EmailControllerTest
    {
        private readonly EMailController _eMailController;
        private readonly IEMailService _emailService;
        private readonly EmailSettingsModel _emailSettingsModel;
        private readonly IRepository<Subscriber> _subcriberRepository;
        private readonly IRepository<EmailTemplate> _templateRepository;
        private readonly ILogger _logger;
        private readonly DbContext _dBContext;


        public EmailControllerTest(IRepository<Subscriber> subcriberRepository, IRepository<EmailTemplate> templateRepository)
        {
            _emailSettingsModel = new EmailSettingsModel();
            _emailSettingsModel = new EmailSettingsModel();
            _eMailController = new EMailController(_emailService);
        }

        [Fact]
        public void GetWhenCalledSendMail()
        {
            //Arrange
            var requestModel = new RequestModel()
            {
                EMailAdrress = "ted@mail.com",
                MailOwner = "Ted",
                Body = "message",
                Topic = "this topic"
            };

            //Act
            var createRequest = _eMailController.SendEmailAsync(requestModel);
            var result = createRequest.Result.ToString();

            //Assert
            var expectedMessage = $"Mail was sent to {requestModel.MailOwner}";
            Assert.Equal(expectedMessage, result);
        }

        [Fact]
        public void GetWhenCalledAddNewTemplate()
        {
            //Arrange
            var templateModel = new TemplateModel()
            {
                Template = "Template",
                Topic = "Topic"
            };

            //Act
            var createRequest = _eMailController.AddNewTemplateAsync(templateModel);
            var result = createRequest.Result.ToString();

            //Assert
            var expectedMessage = "New template was added";
            Assert.Equal(expectedMessage, result);

        }

        [Fact]
        public void GetWhenCalledRegistrateNewSubscriber()
        {
            //Arrange
            var subscriber = new SubscriberModel()
            {
                Email = "subscriber@mail.com",
                Name = "John"
            };

            //Act
            var createRequest = _eMailController.RegistrateNewSubscriberAsync(subscriber);
            var result = createRequest.Result.ToString();

            //Assert
            var expectedMessage = "New subscriber was added";
            Assert.Equal(expectedMessage, result);
        }
    }
}
