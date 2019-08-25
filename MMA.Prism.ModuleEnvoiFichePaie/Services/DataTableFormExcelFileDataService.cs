using MMA.Prism.ModuleEnvoiFichePaie.Services.Contracts;
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
    public class DataTableFormExcelFileDataService : IDataTableFormExcelFileDataService
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
        public DataTable GetDataTableFromExcelFile(string file)
        {
            _logger.Debug($"==> Debut récupération des données dans un fichier Excel et convertion en DataTable.");
            DataTable dt = new DataTable();

            _logger.Debug($"==> Début récupération de la chaine de connection OLEDB");
            string connectionString = Provider.GetConnectionString(file);
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
                        {
                            continue;
                        }

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
    }
}
