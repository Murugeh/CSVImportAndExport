using DAL;
using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data;

namespace CustomerBLL
{
    public class CustomerManager
    {
        ICustomerDAL objDAL = CustomerDAL.GetCSVData();
        public List<Customer> GetCSVData(int Page,int PageSize)
        {

            return objDAL.GetCSVData(Page, PageSize);
        }
        public int GetCSVDataTotalCount()
        {
            return objDAL.GetCSVDataTotalCount();
        }
        public DataSet GetCSVDataExport()
        {
            return objDAL.GetCSVDataExport();
        }
        public int ImportDataTableToDB(DataTable dt)
        {
            return objDAL.ImportDataTableToDB(dt);
        }
    }
}
