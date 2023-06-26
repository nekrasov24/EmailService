using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMailService.Models
{
    public class AppContext : DbContext
    {
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {

        }
    }
}
