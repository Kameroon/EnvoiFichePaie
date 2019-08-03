using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.ModuleEnvoiFichePaie.Services
{
    public class DataService
    {
        /*
         * A installer https://www.microsoft.com/en-us/download/details.aspx?id=13255
         * */

        public static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// -- --
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromExcelFile(string file)
        {
            _logger.Debug($"==> Debut récupération des données dans un fichier Excel et convertion en DataTable.");
            DataTable dt = new DataTable();

            _logger.Debug($"==> Début récupération de la chaine de connection OLEDB");
            string connectionString = GetConnectionString(file);
            _logger.Debug($"==> Fin récupération de la chaine de connection OLEDB");

            try
            {
                _logger.Debug($"==> Connection à la source de données");
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    conn.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = conn;

                    // Get all Sheets in Excel File
                    DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    // Loop through all Sheets to get data
                    foreach (DataRow dr in dtSheet.Rows)
                    {
                        string sheetName = dr["TABLE_NAME"].ToString();

                        if (!sheetName.EndsWith("$"))
                            continue;

                        // Get all rows from the Sheet
                        cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                        dt.TableName = sheetName;

                        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                        da.Fill(dt);

                        break;
                    }

                    cmd = null;
                }

                _logger.Debug($"==> Fin récupération des données dans un fichier Excel et convertion en DataTable.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            return dt;
        }

        /// <summary>
        /// --   --
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static string GetConnectionString(string file)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();
            string extension = file.Split('.').Last();
            StringBuilder sb = new StringBuilder();

            //  value="Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};
            // Extended Properties='Excel 8.0;HDR=YES'"
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
