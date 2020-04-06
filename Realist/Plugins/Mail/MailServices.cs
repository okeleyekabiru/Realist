using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Plugins.Mail
{
 
      public class EmailService :IMailService
      {
          private readonly IConfigurationSection _emailConfig;
          private readonly ILogger<EmailService> _logger;
          private readonly IHttpContextAccessor _accessor;

          public EmailService(IConfiguration config, ILogger<EmailService> logger,IHttpContextAccessor accessor)
          {
              _emailConfig = config.GetSection("Email");
              _logger = logger;
              _accessor = accessor;
          }
          public void SendMail(string email, string message, string subject)
          {
              if (string.IsNullOrEmpty(email))
              {
                  email = _emailConfig.GetValue<string>("Developer");
              }
              try
              {
                  // Credentials
                  var credentials = new NetworkCredential(_emailConfig.GetValue<string>("Address"),
                      _emailConfig.GetValue<string>("Password"));

                  // Mail message
                  var mail = new MailMessage()
                  {
                      From = new MailAddress("noreply@realist.com"),
                      Subject = subject,
                      Body = message
                  };

                  mail.IsBodyHtml = true;

                  mail.To.Add(new MailAddress(email));

                  // Smtp client
                  var client = new SmtpClient()
                  {
                      Port = 587,
                      DeliveryMethod = SmtpDeliveryMethod.Network,
                      UseDefaultCredentials = false,
                      Host = "smtp.gmail.com",
                      EnableSsl = true,
                      Credentials = credentials
                  };

                  client.Send(mail);
              }
              catch (Exception e)
              {
                  _logger.LogInformation($"Something went wrong, unable to send email to {email} on {DateTime.Now}");
              }
          }
   

   

        public string ErrorMessage(string message)
        {
            var builder = new StringBuilder();
            builder.Append(
                "<!DOCTYPE html>" +
                "<html lang='en'>" +
                "<head>  " +
                " <meta charset='UTF-8'> " +
                "   <meta name='viewport' content='width=device-width', initial-scale=1.0>" +
                "   <title>Document</title>" +
                "</head>" +
                "<body>" +
                " <h1 style='color: red;text-decoration: underline;'>Error Alert</h1> " +
                $"  <p>{message}</p>" +
                "</body>" +
                "</html>");
            return builder.ToString();
        }

        public void VerifyEmail(string email,string message)
        {
            string host = _accessor.HttpContext.Request.Host.Value;
            SendMail(email,  $"Please confirm your account by clicking <a href='{host+ "/api/user/confirmation?Token=" + message}'>here</a>", "Confirm your account");
        }
    }
}
