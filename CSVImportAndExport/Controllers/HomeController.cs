using CustomerBLL;
using CustomerModel;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CSVImportAndExport.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            try
            {
                List<Customer> lstCustomer = new List<Customer>();
                CustomerManager objManager = new CustomerManager();

                int totalCount = objManager.GetCSVDataTotalCount();
                var pager = new PagerModel(totalCount, page);
                lstCustomer = objManager.GetCSVData(pager.CurrentPage, pager.PageSize);
                var CustomerDetails = new CustomerDetailsInfo()
                {
                    CustomerInfo = lstCustomer,
                    Pager = pager
                };
                return View(CustomerDetails);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage");
                //Log Exceptions
            }

        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile, int? page)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                try
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(postedFile.FileName);
                    string extension = Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    DataTable dt = new DataTable();
                    CustomerManager objManager = new CustomerManager();
                    dt.Columns.AddRange(new DataColumn[4] { new DataColumn("CustomerName", typeof(string)),
                                new DataColumn("City", typeof(string)),
                                new DataColumn("State",typeof(string)),
                                new DataColumn("Country",typeof(string))});

                    string csvDataOld = System.IO.File.ReadAllText(filePath);
                    string csvData = csvDataOld + "\r\n";
                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            dt.Rows.Add();
                            int i = 0;
                            foreach (string cell in row.Split(','))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell;
                                i++;
                            }
                        }
                    }
                    int retvalue = objManager.ImportDataTableToDB(dt);
                    if(retvalue==1)
                    {
                        TempData["FileUploadSuccess"] = "SuccessMsg";
                    }
                    else
                    {
                        TempData["Failure"] = "Failure";
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("ErrorPage");
                    //Log Exceptions
                }

            }
            return RedirectToAction("Index");
        }
        public ActionResult ExportCSV()
        {
            try
            {
                var sb = new StringBuilder();
                List<Customer> lstCustomer = new List<Customer>();
                CustomerManager objManager = new CustomerManager();
                int totalCount = objManager.GetCSVDataTotalCount();
                if (totalCount > 0)
                {
                    DataSet dsresult = objManager.GetCSVDataExport();
                    DataTable dt = dsresult.Tables[0];
                    string path = Server.MapPath("~\\Downloads\\");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    lstCustomer = (from DataRow dr in dt.Rows
                                   select new Customer()
                                   {
                                       CustomerName = Convert.ToString(dr["CustomerName"]),
                                       City = dr["City"].ToString(),
                                       State = dr["State"].ToString(),
                                       Country = dr["Country"].ToString()
                                   }).ToList();
                    var list = lstCustomer;
                    sb.AppendFormat("{0},{1},{2},{3}", "CustomerName", "City", "State", "Country");
                    sb.AppendFormat("\r\n");
                    foreach (var item in list)
                    {
                        sb.AppendFormat("{0},{1},{2},{3}", item.CustomerName, item.City, item.State, item.Country);
                    }
                    var response = System.Web.HttpContext.Current.Response;
                    response.BufferOutput = true;
                    response.Clear();
                    response.ClearHeaders();
                    response.ContentEncoding = Encoding.Unicode;
                    response.AddHeader("content-disposition", "attachment;filename=CustomerInfo.csv ");
                    response.ContentType = "text/plain";
                    response.Write(sb.ToString());
                    response.End();
                }
                else
                {
                    TempData["ExportFail"] = "NoRecords";
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("ErrorPage");
                //Log Exceptions
            }
            return RedirectToAction("Index");
        }
        public ActionResult ErrorPage()
        {
            ViewBag.Message = "Error page.";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "ErrorPage";

            return View();
        }
       

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}