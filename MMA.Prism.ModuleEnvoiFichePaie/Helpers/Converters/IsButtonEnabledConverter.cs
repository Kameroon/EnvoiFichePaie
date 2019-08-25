using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace MMA.Prism.ModuleEnvoiFichePaie.Helpers.Converters
{
    public class IsButtonEnabledConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parsingOk = Regex.IsMatch(value.ToString(), Supplier.MAILPATTERN,
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));

            return parsingOk ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #region -- MultivalueConverter : IMultiValueConverter  --
        //public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        //{
        //    bool res = true;

        //    foreach (object val in values)
        //    {
        //        if (!string.IsNullOrEmpty(val as string))
        //        {
        //            var parsingOk = Regex.IsMatch(val.ToString(),
        //                                 @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        //                                 @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        //                             RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

        //            if (!parsingOk)
        //            {
        //                res = false;
        //            }
        //            else
        //            {
        //                res = true;
        //            }
        //        }                
        //    }

        //    return res;
        //}

        //public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        //{
        //    throw new NotImplementedException();
        //}

        /*
         *  <!--<Button.Style>
                            <Style TargetType="Button">
                                <Setter Property="IsEnabled" Value="False"/>
                                <Style.Triggers>
                                    <MultiDataTrigger>
                                        <MultiDataTrigger.Conditions>
                                            <Condition Binding="{Binding Path=(Validation.HasError), ElementName=textBlokAdminEmail}"
                                                       Value="False"/>
                                            <Condition Binding="{Binding ElementName=textBlokAdminEmail, Path=Text.Length, Converter={StaticResource IsButtonEnabledConverter}}"
                                                       Value="False" />
                                        </MultiDataTrigger.Conditions>
                                        <Setter Property="IsEnabled" Value="True"/>
                                    </MultiDataTrigger>
                                </Style.Triggers>
                            </Style>
                        </Button.Style>--> 
         * */
        #endregion
    }
}
