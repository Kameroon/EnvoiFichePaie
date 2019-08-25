
using System.Data;

namespace MMA.Prism.ModuleEnvoiFichePaie.Services.Contracts
{
    public  interface IDataTableFormExcelFileDataService
    {
        DataTable GetDataTableFromExcelFile(string file);
    }
}
