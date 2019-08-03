using MMA.Prism.ModuleEnvoiFichePaie.MVVM.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MMA.Prism.ModuleEnvoiFichePaie.Helpers
{
    public class SendEmailHelper
    {
        public static bool SendEmail(IEmailMessage emailMessage)
        {            
            bool result = false;
            var de = ConfigurationManager.AppSettings["smtpUser"];
            try
            {  
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(emailMessage.AdminEmail);
                    mailMessage.To.Add(emailMessage.ToEmail);
                    mailMessage.Subject = emailMessage.Suject;

                    string currentMonth = DateTime.Now.ToString("MMMM").ToUpper();
                    string htmlTemplate = File.ReadAllText(emailMessage.MailBody, Encoding.Default);
                    string body = null;

                    mailMessage.BodyEncoding = Encoding.Default;
                    mailMessage.IsBodyHtml = true;

                    mailMessage.Body = MailConsolidateHelper.BuilBody(body, htmlTemplate, currentMonth, emailMessage.ToEmail);

                    if (!string.IsNullOrWhiteSpace(emailMessage.Bcc))
                        mailMessage.Bcc.Add(emailMessage.Bcc);
                    if (!string.IsNullOrWhiteSpace(emailMessage.Cc))
                        mailMessage.CC.Add(emailMessage.Cc);

                    Attachment attachment;
                    attachment = new Attachment(emailMessage.FilePath);
                    mailMessage.Attachments.Add(attachment);

                    using (SmtpClient smtpClient = new SmtpClient(Supplier.SMTP_CREDENTIAL, 587))
                    {                     
                        smtpClient.Port = 587;
                        smtpClient.Credentials = new NetworkCredential(
                            Supplier.USERNAME_CREDENTIAL,
                            Supplier.PASSWORD_CREDENTIAL);

                        smtpClient.EnableSsl = true;

                        // -- Si cest un test avant envoie m'enoyer tous les mails --
                        if (emailMessage.IsPreviewMail)
                        {
                            MailConsolidateHelper.CheckCcAndBcc(emailMessage.AdminEmail, mailMessage);
                        }

                        smtpClient.Send(mailMessage);

                        result = true;
                    }
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return result;
        }
    }
}
