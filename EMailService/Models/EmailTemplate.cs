using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMailService.Models
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }

        public string Template { get; set; }

        public string Topic { get; set; }
    }
}
