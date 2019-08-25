using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie.Services
{
    public static class Provider
    {
        /// <summary>
        /// --   --
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetConnectionString(string file)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            string extension = file.Split('.').Last();
            StringBuilder sb = new StringBuilder();

            try
            {
                if (extension == "xls")
                {
                    //Excel 2003 and Older
                    props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
                    props["Extended Properties"] = "Excel 8.0";
                }
                else if (extension == "xlsx")
                {
                    //Excel 2007, 2010, 2012, 2013
                    props["Provider"] = "Microsoft.ACE.OLEDB.12.0;";
                    props["Extended Properties"] = "Excel 12.0 XML";
                }
                else
                    throw new Exception(string.Format("error file: {0}", file));

                props["Data Source"] = file;

                foreach (KeyValuePair<string, string> prop in props)
                {
                    sb.Append(prop.Key);
                    sb.Append('=');
                    sb.Append(prop.Value);
                    sb.Append(';');
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return sb.ToString();
        }
    }
}
