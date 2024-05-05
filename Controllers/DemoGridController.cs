using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CrudAppliction.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860 

namespace CrudApplication.Controllers
{
    
    public class DemoGridController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration configuration;

        public DemoGridController(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }
        // GET: /<controller>/  
        public IActionResult Profile()
        {
           return View();
        }

        [AllowAnonymous]
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult ShowGrid()
        {
            return View();
        }
        public JsonResult GetCustomerList()
        {
            var data = _context.CustomerTBs.ToList();
            return new JsonResult(data);
        }
        //public IActionResult LoadData()
        //{
        //    try
        //    {
        //        var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
        //        // Skiping number of Rows count  
        //        var start = Request.Form["start"].FirstOrDefault();
        //        // Paging Length 10,20  
        //        var length = Request.Form["length"].FirstOrDefault();
        //        // Sort Column Name  
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        // Sort Column Direction ( asc ,desc)  
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        // Search Value from (Search box)  
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        //Paging Size (10,20,50,100)  
        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;
        //        // Getting all Customer data  
        //        var customerData = (from tempcustomer in _context.CustomerTB
        //                            select tempcustomer);
        //        //Sorting  
        //        if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
        //        {
        //            customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
        //        }
        //        //Search  
        //        if (!string.IsNullOrEmpty(searchValue))
        //        {
        //            customerData = customerData.Where(m => m.FirstName == searchValue);
        //        }

        //        //total number of rows count   
        //        recordsTotal = customerData.Count();
        //        //Paging   
        //        var data = customerData.Skip(skip).Take(pageSize).ToList();
        //        //Returning Json Data  
        //        return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //    }

        [HttpPost]
        public JsonResult AddEmployee(CustomerTB customer)
        {
            var emp = new CustomerTB()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                Country = customer.Country,
                state = customer.state,
                City = customer.City,
                Phoneno = customer.Phoneno,
                Email = customer.Email,
                Pincode = customer.Pincode
            };
            _context.CustomerTBs.Add(emp);
            _context.SaveChanges();
            return new JsonResult("data is saved");
        }


        public JsonResult Delete(int id)
        {
            var emp = _context.CustomerTBs.Where(e => e.CustomerID == id).SingleOrDefault();
            _context.CustomerTBs.Remove(emp);
            _context.SaveChanges();
            return new JsonResult("data deleted!");
        }
        [HttpGet]
        public JsonResult Edit(int id)
        {
            var emp = _context.CustomerTBs.Where(x => x.CustomerID == id).FirstOrDefault();

            return new JsonResult(emp);
        }
        [HttpPost]
        public IActionResult Update(CustomerTB model)
        {
            _context.CustomerTBs.Update(model);
            _context.SaveChanges();
            return RedirectToAction("ShowGrid");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn(Registration model)
        {
            var data = _context.Register.Where(e => e.Email == model.Email).SingleOrDefault();
            if (data != null)
            {
                bool isvalid = (data.Email == model.Email && data.Password == model.Password);
                if (isvalid)
                {
                    var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, model.Email) },
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    HttpContext.Session.SetString("Email", model.Email);
                    return RedirectToAction("ShowGrid", "DemoGrid");
                }

                else
                {
                    TempData["errormessage"] = "Invalid Password";
                }

            }
            else
            {
                TempData["message"] = "Username not found";
                return View(model);
            }

            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Dashboard", "DemoGrid");
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignUp(Registration model)
        {
            var data = new Registration()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Password = model.Password,
            };
            _context.Add(data);
            _context.SaveChanges();
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult ImportExcelFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportExcelFile(IFormFile formfile)
        {
            try
            {
                var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcelFile");
                if (!Directory.Exists(mainPath))
                {
                    Directory.CreateDirectory(mainPath);
                }
                var filePath = Path.Combine(mainPath, formfile.FileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    formfile.CopyTo(stream);
                }
                var fileName = formfile.FileName;
                string extension = Path.GetExtension(fileName);
                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls":
                        conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties= 'Excel 8.0; HDR=Yes;'";
                        break;
                    case ".xlsx":
                        conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties= 'Excel 8.0; HDR=Yes;'";
                        break;
                }
                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);
                using (OleDbConnection conExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = conExcel;
                            conExcel.Open();
                            DataTable dtExcelSchema = conExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            cmdExcel.CommandText = "SELECT * FROM [" + SheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            conExcel.Close();
                        }
                    }
                }

                conString = configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(con))
                    {
                        sqlbulkCopy.DestinationTableName = "CustomerTB";
                        sqlbulkCopy.ColumnMappings.Add("CustomerId", "CustomerId");
                        sqlbulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                        sqlbulkCopy.ColumnMappings.Add("LastName", "LastName");
                        sqlbulkCopy.ColumnMappings.Add("Address", "Address");
                        sqlbulkCopy.ColumnMappings.Add("Country", "Country");
                        sqlbulkCopy.ColumnMappings.Add("City", "City");
                        sqlbulkCopy.ColumnMappings.Add("state", "state");
                        sqlbulkCopy.ColumnMappings.Add("Phoneno", "Phoneno");
                        sqlbulkCopy.ColumnMappings.Add("Email", "Email");
                        sqlbulkCopy.ColumnMappings.Add("Pincode", "Pincode");
                        con.Open();
                        sqlbulkCopy.WriteToServer(dt);
                        con.Close();

                    }
                }
                ViewBag.message = "File Imported Successfully, Data Saved into Database.";
                return RedirectToAction("ShowGrid");

            }

            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return View();

        }
       
    }
}



