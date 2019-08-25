using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMA.Prism.ModuleEnvoiFichePaie.Services;
using MMA.Prism.ModuleEnvoiFichePaie.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA.Prism.Test
{
    [TestClass]
    public class DataTableFormExcelFileDataServiceTest
    {
        private DataTable dataTable;

        public DataTableFormExcelFileDataServiceTest()
        {
            Initialize();
        }

        /// <summary>
        /// --  --
        /// </summary>
        private void Initialize()
        {
            dataTable = new DataTable();
            dataTable = GetTable();
        }

        /// <summary>
        /// -- Give back datatable --
        /// </summary>
        /// <returns></returns>
        private DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add("Nom salarie", typeof(string));
            table.Columns.Add("Chemin du fichier", typeof(string));

            // Here we add five DataRows.
            table.Rows.Add("Indocin", @"C:\Users\Sweet Family\Desktop\Mail Afersys");
            table.Rows.Add("Enebrel", @"C:\Users\Sweet Family\Desktop\Mail Afersys");
            table.Rows.Add("Hydralazine", @"C:\Users\Sweet Family\Desktop\Mail Afersys");
            table.Rows.Add("Combivent", @"C:\Users\Sweet Family\Desktop\Mail Afersys");
            table.Rows.Add("Hydralazine", @"C:\Users\Sweet Family\Desktop\Mail Afersys");
            table.Rows.Add("Combivent", @"C:\Users\Sweet Family\Desktop\Mail Afersys");

            return table;
        }

        [TestMethod]
        public void GetDataTableFromExcelFile_ShoudWork()
        {
            // Arrange  
            DataTableFormExcelFileDataService dataService = new DataTableFormExcelFileDataService();
            DataTable dt = new DataTable();
            int countDt = dt.Rows.Count;
            string file = @"C:\Users\Sweet Family\Desktop\Mail Afersys\TestAfersys_Test1.xlsx";

            // Act
            var data = dataService.GetDataTableFromExcelFile(file);
            int countData = data.Rows.Count;

            // Assert
            Assert.IsNotNull(data);
            Assert.AreEqual(countDt, countData);
        }

        [TestMethod]
        public void GetDataTableFromExcelFile_ShoudNotWork()
        {
            // Arrange
            DataTableFormExcelFileDataService dataService = new DataTableFormExcelFileDataService();
            int countDt = dataTable.Rows.Count;
            string file = @"C:\Users\Sweet Family\Desktop\Mail Afersys\TestAfersysPCFixe.xlsx";

            // Act
            var data = dataService.GetDataTableFromExcelFile(file);
            int countData = data.Rows.Count;

            // Assert
            Assert.IsNotNull(data);
            Assert.AreEqual(countDt, countData);
        }
    }
}
