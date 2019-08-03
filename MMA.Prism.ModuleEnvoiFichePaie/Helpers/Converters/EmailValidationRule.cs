using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie.Helpers.Converters
{
    public class EmailValidationRule : System.Windows.Controls.ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var validationResult = new System.Windows.Controls.ValidationResult(true, null);
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    //var regex = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
                    //var parsingOk = regex.IsMatch(value.ToString());

                    var parsingOk = Regex.IsMatch(value.ToString(),
                       @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                   RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                    if (!parsingOk)
                    {
                        validationResult = new System.Windows.Controls.ValidationResult(false, "Vous devez saisir une adresse email valide !");
                    }
                }
            }
            return validationResult;
        }
    }

}
