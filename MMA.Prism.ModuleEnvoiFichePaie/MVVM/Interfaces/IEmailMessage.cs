using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie.MVVM.Interfaces
{
    public interface IEmailMessage
    {
        string ToEmail { get; set; }
        string MailBody { get; set; }
        string Suject { get; set; }
        string FilePath { get; set; }
        string Bcc { get; set; }
        string Cc { get; set; }
        string AdminEmail { get; set; }
        bool IsPreviewMail { get; set; }
    }
}
