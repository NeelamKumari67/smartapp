using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using System.IO;
using System.Text;
using CrudAppliction.Models;
using System.Drawing.Printing;

namespace CrudApplication.Controllers
{
    public class ExcelController : Controller
    {
        private readonly DatabaseContext _context;

        public ExcelController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult ExportToExcel()
        {
            try
            {
                var data = _context.CustomerTBs.ToList();
                if (data != null & data.Count > 0)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ToConvertDataTable(data.ToList()));
                        using (MemoryStream ms = new MemoryStream())
                        {
                            wb.SaveAs(ms);
                            string fileName = $"CustomerTB_{DateTime.Now.ToString("dd/MM/yyyy")}.xlsx";
                            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocuments.spreadsheetml.sheet", fileName);
                        }
                    }
                }
                TempData["Error"] = "Data not found";
            }
            catch (Exception ex)
            {

            }
            return View("ShowGrid");
        }
        public DataTable ToConvertDataTable<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] propinfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in propinfo)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[propinfo.Length];
                for (int i = 0; i < propinfo.Length; i++)
                {
                    values[i] = propinfo[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}


       
     

        


