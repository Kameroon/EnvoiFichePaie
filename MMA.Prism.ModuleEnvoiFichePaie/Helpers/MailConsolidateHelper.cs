using MMA.Prism.ModuleEnvoiFichePaie.MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie.Helpers
{
    public static class MailConsolidateHelper
    {
        /// <summary>
        /// --   --
        /// </summary>
        /// <param name="body"></param>
        /// <param name="htmlTemplate"></param>
        /// <param name="currentMonth"></param>
        /// <param name="toEmail"></param>
        /// <returns></returns>
        public static string BuilBody(string body, string htmlTemplate,
            string currentMonth, string toEmail)
        {
            body = htmlTemplate.Replace("{E}", toEmail.Split('@')[0]);
            body = body.Replace("{C}", currentMonth);
            body += "\n\rCeci est un message automatique.";

            return body;
        }

        /// <summary>
        /// --  --
        /// </summary>
        /// <param name="adminEmail"></param>
        /// <param name="mailMessage"></param>
        public static void CheckCcAndBcc(string adminEmail, MailMessage mailMessage)
        {
            Console.WriteLine("Destiné à : " + mailMessage.To.FirstOrDefault());
            mailMessage.Subject += " - To : " + mailMessage.To.FirstOrDefault();
            mailMessage.To.Clear();
            mailMessage.To.Add(adminEmail);
            mailMessage.CC.Clear();
            mailMessage.Bcc.Clear();
            mailMessage.Bcc.Add(adminEmail);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void BuildMailDetail(IEmailMessage emailMessage, string corpsDuMail,
            string sujet, string itemValue, string itemKey, string bcc,
            string cc, string adminEmail, bool isPreviewEmail)
        {
            emailMessage.MailBody = corpsDuMail;
            emailMessage.Suject = sujet;
            emailMessage.FilePath = itemValue;
            emailMessage.ToEmail = itemKey;
            emailMessage.Bcc = bcc;
            emailMessage.Cc = cc;
            emailMessage.AdminEmail = adminEmail;
            emailMessage.IsPreviewMail = isPreviewEmail;
        }

        ///// <summary>
        ///// -- Check de la saisie user de Bccmail et Ccmail --
        ///// </summary>
        ///// <param name="email"></param>
        //public static void ValidateEmail(string email)
        //{
        //    // -- $"==> Vérification de email saisie." --
        //    if ((!string.IsNullOrEmpty(email) && !RegexMailUtilities.IsValidEmail(email)) &&
        //        !RegexMailUtilities.IsValidEmail(email))
        //    {
        //        CanContinuousSendMail = false;
        //        ErrorMessage = string.Format(ErrorMessageLabels.ImpossibleToSendMailMsg, email);
        //        _logger.Error(ErrorMessage);

        //        _dialogService.ShowMessage(ErrorMessage, "ERROR",
        //                                   MessageBoxButton.OK,
        //                                   MessageBoxImage.Error,
        //                                   MessageBoxResult.Yes);
        //    }
        //    else
        //    {
        //        _logger.Debug($"==> Poursuite de l'envoie de mail après vérification de l'email.");
        //        CanContinuousSendMail = true;
        //    }
        //}
    }
}
