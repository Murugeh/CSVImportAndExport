using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface ICustomerDAL
    {
        List<Customer> GetCSVData(int Page, int PageSize);
        int GetCSVDataTotalCount();
        DataSet GetCSVDataExport();
        int ImportDataTableToDB(DataTable dt);
    }
}
