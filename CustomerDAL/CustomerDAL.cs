using DAL;
using IDAL;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CustomerDAL: ICustomerDAL
    {
        #region SP Params
        const string PARAM_PAGE = "@Page";
        const string PARAM_PAGESIZE = "@PageSize";

        #endregion

        #region Stored Procedures
        const string SP_GET_CSVData = "GetCSVData";
        const string SP_GET_CSVDataTotalCount = "CSVDataTotalCount";
        const string SP_GET_CSVDataExport = "CSVDataExport";
        #endregion

        public static ICustomerDAL GetCSVData()
        {
            return new CustomerDAL();
        }

        public List<Customer> GetCSVData(int Page, int PageSize)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter(PARAM_PAGE,SqlDbType.Int),
                new SqlParameter(PARAM_PAGESIZE,SqlDbType.Int)
             };
            parms[0].Value = Page;
            parms[1].Value = PageSize;

            SqlDataReader reader = null;
            List<Customer> lstCustomer = new List<Customer>();
            
            try
            {
                string conString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                reader = SqlHelper.ExecuteReader(conString, CommandType.StoredProcedure, SP_GET_CSVData, parms);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Customer objCustomer = new Customer();
                        if (reader["CustomerName"] != DBNull.Value)
                            objCustomer.CustomerName = Convert.ToString(reader["CustomerName"]);
                        if (reader["City"] != DBNull.Value)
                            objCustomer.City = Convert.ToString(reader["City"]);
                        if (reader["State"] != DBNull.Value)
                            objCustomer.State = Convert.ToString(reader["State"]);
                        if (reader["Country"] != DBNull.Value)
                            objCustomer.Country = Convert.ToString(reader["Country"]);

                        lstCustomer.Add(objCustomer);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return lstCustomer;
        }
        public int GetCSVDataTotalCount()
        {
            string conString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            int totalCount = (int)SqlHelper.ExecuteScalar(conString, CommandType.StoredProcedure, SP_GET_CSVDataTotalCount);
            return totalCount;
        }
        public DataSet GetCSVDataExport()
        {
            DataSet ds = null;
            string conString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            try
            {
                ds = SqlHelper.ExecuteDataset(conString, CommandType.StoredProcedure, SP_GET_CSVDataExport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
    }
}
