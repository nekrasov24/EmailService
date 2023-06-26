using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMailService.Models;
using EMailService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMailService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EMailController : ControllerBase
    {
        private readonly IEMailService _emailService;

        public EMailController(IEMailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmailAsync([FromForm] RequestModel requestModel)
        {
            try
            {
                var result = await _emailService.SendEmailAsync(requestModel);
                return Ok(result);
                
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("addnewtemplate")]
        public async Task<IActionResult> AddNewTemplateAsync([FromBody] TemplateModel requestModel)
        {
            try
            {
                var result = await _emailService.AddNewTemplateAsync(requestModel);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("registrate")]
        public async Task<IActionResult> RegistrateNewSubscriberAsync([FromBody] SubscriberModel requestModel)
        {
            try
            {
                var result = await _emailService.RegistrateNewSubscriberAsync(requestModel);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}