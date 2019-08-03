using MMA.Prism.ModuleEnvoiFichePaie.MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie.MVVM.Models
{
    public class EmailMessage : IEmailMessage
    {
        public string ToEmail { get; set; }
        public string MailBody { get; set; }
        public string Suject { get; set; }
        public string FilePath { get; set; }
        public string Bcc { get; set; }
        public string Cc { get; set; }
        public string AdminEmail { get; set; }
        public bool IsPreviewMail { get; set; }

        public EmailMessage()
        {

        }
    }
}
