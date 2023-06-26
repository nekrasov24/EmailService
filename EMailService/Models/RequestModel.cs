using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMailService.Models
{
    public class RequestModel
    {
        [Required(ErrorMessage = "Email adress is required")]
        public string EMailAdrress { get; set; }

        public string MailOwner { get; set; }

        [Required(ErrorMessage = "Topic is required")]
        public string Topic { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }

        public List<IFormFile> Attachments { get; set; }
    }
}
