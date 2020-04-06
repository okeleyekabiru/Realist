using System;
using System.Collections.Generic;
using System.Text;

namespace Plugins.Mail
{
   public interface IMailService
   {
       void SendMail( string email, string message, string subject);
       string ErrorMessage(string message);
       void VerifyEmail(string email, string message);
   }
}
