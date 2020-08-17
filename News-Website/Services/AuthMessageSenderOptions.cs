using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace News_Website.Services
{
    public class AuthMessageSenderOptions
    {
        public AuthMessageSenderOptions(IConfiguration config)
        {
            SendGridUser = config.GetSection("SendGrid").GetSection("SendGridUser").Value;
            SendGridKey = config.GetSection("SendGrid").GetSection("SendGridKey").Value;
        }
        public AuthMessageSenderOptions() { }
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }

}
