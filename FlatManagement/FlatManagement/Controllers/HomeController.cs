using FlatManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        const string APARTCODEVAR = "_ApartCodeSession";
        const string APARTCOMPANYNAME = "_ApartCompanyName";
        const string APARTCOMPANYLOGO = "_ApartCompanyLogo";

        private readonly IStringLocalizer<HomeController> _stringLocalizer;

        private readonly ILogger<HomeController> _logger;

        private readonly FlatDBContext _context;

        public HomeController(ILogger<HomeController> logger, FlatDBContext context, IStringLocalizer<HomeController> stringLocalizer)
        {
            _logger = logger;
            _context = context;
            _stringLocalizer = stringLocalizer;
        }



        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index(string SearchDate)//, DateTime? startdate, DateTime? enddate
        {
            ViewData["PageTitle"] = _stringLocalizer["page.title"].Value;
            ViewData["PageDesc"] = _stringLocalizer["page.description"].Value;
            try
            {
                string userName = User.Identity.Name;
                var getApartCodeName = (from u in _context.Users
                                        where u.IsActive == true && u.UserName == userName
                                        select u.ApartCodeName).FirstOrDefault();
                var getCompanyName = (from u in _context.CompanyInfo
                                      where u.IsActive == true && u.ApartCodeName == getApartCodeName
                                      select u.CompanyName).FirstOrDefault();
                var getCompanyLogo = (from u in _context.CompanyInfo
                                      where u.IsActive == true && u.ApartCodeName == getApartCodeName
                                      select u.LogoUri).FirstOrDefault();

                //HttpContext.Response.Cookies.Append("COMCODE", getApartCodeName);
                //HttpContext.Response.Cookies.Append("COMNAME", getCompanyName);
                //HttpContext.Response.Cookies.Append("COMLOGO", getCompanyLogo);

                if (!HttpContext.Request.Cookies.ContainsKey("COMCODE"))
                {
                    HttpContext.Response.Cookies.Append("COMCODE", getApartCodeName);
                    HttpContext.Response.Cookies.Append("COMNAME", getCompanyName);
                    HttpContext.Response.Cookies.Append("COMLOGO", getCompanyLogo);
                    //HttpContext.Response.Cookies.Append("user_id", "1");
                    //HttpContext.Response.Cookies.Append("first_request", DateTime.Now.ToString());
                    //return Content("Welcome, new visitor!");
                }
                else
                {
                    HttpContext.Session.SetString(APARTCODEVAR, getApartCodeName);
                    HttpContext.Session.SetString(APARTCOMPANYNAME, getCompanyName);
                    HttpContext.Session.SetString(APARTCOMPANYLOGO, getCompanyLogo);

                    ViewBag.CodeName = HttpContext.Request.Cookies["COMCODE"];
                    ViewBag.CompanyName = HttpContext.Request.Cookies["COMNAME"];
                    ViewBag.CompanyLogo = HttpContext.Request.Cookies["COMLOGO"];
                    //DateTime firstRequest = DateTime.Parse(HttpContext.Request.Cookies["first_request"]);
                    //return Content("Welcome back, user! You first visited us on: " + firstRequest.ToString());
                }



                
                //ViewBag.CodeName = HttpContext.Session.GetString(APARTCODEVAR);
                //ViewBag.CompanyName = HttpContext.Session.GetString(APARTCOMPANYNAME);
                //ViewBag.CompanyLogo = HttpContext.Session.GetString(APARTCOMPANYLOGO);
            }catch(Exception ex)
            {
                ViewBag.CodeName = "";
                ViewBag.CompanyName = "";
                ViewBag.CompanyLogo = "no_image_apartment.png";
            }

            int searchDateParam = Convert.ToInt32(SearchDate);
            
            //BillCount
            ViewBag.BillCount = GetTotalBill(searchDateParam);


            //MaintenanceCount
            ViewBag.GetTotalMaintenance = GetTotalMaintenance(searchDateParam);

            // RenewalCount
            ViewBag.GetTotalRenewal = GetTotalRenewal(searchDateParam);

            //MeetingCount

            ViewBag.GetTotalMeeting = GetTotalMeeting(searchDateParam);

            //AmountCollectionCount

            ViewBag.GetTotalCollection = GetTotalCollection(searchDateParam);

            //PendingTaskCount

            ViewBag.GetPendingTask = GetPendingTask(searchDateParam);

            ViewBag.SearchDate = searchDateParam;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public Decimal GetTotalBill(int dayVal = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            int countPendingBill = 0;
            DateTime startdate = DateTime.Now; 
            DateTime enddate = DateTime.Now; 

            if (dayVal >= 0)
            {
                 startdate = DateTime.Now;
                 enddate = startdate.AddDays(dayVal);
                
                countPendingBill = (from row in _context.Bills
                                        where (row.BillStatus == "Initial")
                                        where row.DueDate >= startdate
                                        where row.DueDate <= enddate
                                        where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();

            }else if(dayVal < 0)
            {
                enddate = DateTime.Now;
                startdate = enddate.AddDays(dayVal);
                 

                countPendingBill = (from row in _context.Bills
                                    where (row.BillStatus == "Initial")
                                    where row.DueDate >= startdate
                                    where row.DueDate <= enddate
                                    select row).Count();
            }
            else
            {
                countPendingBill = (from row in _context.Bills
                                    where (row.BillStatus == "Initial")
                                    select row).Count();
            }

            return countPendingBill;
        }


        public Decimal GetTotalMaintenance(int dayVal = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            int countPendinglist = 0;
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;
            if (dayVal >= 0)
            {
                 startdate = DateTime.Now;
                 enddate = startdate.AddDays(dayVal);

                //where(row.BillStatus == "Initial")
                countPendinglist = (from row in _context.Maintenances
                                    where row.MaintenanceDate >= startdate
                                    where row.MaintenanceDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();

            }
            else if (dayVal < 0)
            {
                enddate = DateTime.Now;
                startdate = enddate.AddDays(dayVal);


                countPendinglist = (from row in _context.Maintenances
                                    where row.MaintenanceDate >= startdate
                                    where row.MaintenanceDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else
            {
                countPendinglist = (from row in _context.Maintenances.Where(row=>row.ApartCodeName== APART_CODE_LOCAL_VAR)
                                    select row).Count();
            }

            return countPendinglist;
        }


        //Service Table
        public Decimal GetTotalRenewal(int dayVal = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            int countPendinglist = 0;
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;

            if (dayVal >= 0)
            {
                startdate = DateTime.Now;
                enddate = startdate.AddDays(dayVal);

                //where(row.BillStatus == "Initial")
                countPendinglist = (from row in _context.Services
                                    where row.StartDate >= startdate
                                    where row.EndDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else if (dayVal < 0)
            {
                enddate = DateTime.Now;
                startdate = enddate.AddDays(dayVal);
                countPendinglist = (from row in _context.Services
                                    where row.StartDate >= startdate
                                    where row.EndDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else
            {
                countPendinglist = (from row in _context.Services.Where(row => row.ApartCodeName == APART_CODE_LOCAL_VAR)
                                    select row).Count();
            }

            return countPendinglist;
        }


        //Meeting
        public Decimal GetTotalMeeting(int dayVal = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            int countPendinglist = 0;
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;

            if (dayVal >= 0)
            {
                startdate = DateTime.Now;
                enddate = startdate.AddDays(dayVal);

                //where(row.BillStatus == "Initial")
                countPendinglist = (from row in _context.Resolutions
                                    where row.DueDate >= startdate
                                    where row.DueDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else if (dayVal < 0)
            {
                enddate = DateTime.Now;
                startdate = enddate.AddDays(dayVal);
                countPendinglist = (from row in _context.Resolutions
                                    where row.DueDate >= startdate
                                    where row.DueDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else
            {
                countPendinglist = (from row in _context.Resolutions.Where(row => row.ApartCodeName == APART_CODE_LOCAL_VAR)
                                    select row).Count();
            }

            return countPendinglist;
        }

        //Collection
        public Decimal GetTotalCollection(int dayVal = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            int countPendinglist = 0;
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;


            if (dayVal >= 0)
            {
                startdate = DateTime.Now;
                enddate = startdate.AddDays(dayVal);

                //where(row.BillStatus == "Initial")
                countPendinglist = (from row in _context.CommonFunds
                                    where row.CollectionDate >= startdate
                                    where row.CollectionDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR

                                    select row).Count();
            }
            else if (dayVal < 0)
            {
                enddate = DateTime.Now;
                startdate = enddate.AddDays(dayVal);
                countPendinglist = (from row in _context.CommonFunds
                                    where row.CollectionDate >= startdate
                                    where row.CollectionDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else
            {
                countPendinglist = (from row in _context.CommonFunds.Where(row => row.ApartCodeName == APART_CODE_LOCAL_VAR)
                                    select row).Count();
                
            }

            return countPendinglist;
        }


        //GetPendingTask

        public Decimal GetPendingTask(int dayVal = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            int countPendinglist = 0;
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now; 


            if (dayVal >= 0)
            {
                startdate = DateTime.Now;
                enddate = startdate.AddDays(dayVal);

                //where(row.BillStatus == "Initial")
                countPendinglist = (from row in _context.Resolutions
                                    where row.Status == "InProgress"
                                    where row.DueDate <= startdate
                                    where row.DueDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else if (dayVal < 0)
            {
                enddate = DateTime.Now;
                startdate = enddate.AddDays(dayVal);
                countPendinglist = (from row in _context.Resolutions
                                    where row.Status == "InProgress"
                                    where row.DueDate <= startdate
                                    where row.DueDate <= enddate
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }
            else
            {
                countPendinglist = (from row in _context.Resolutions
                                    where row.Status == "InProgress"
                                    where row.ApartCodeName == APART_CODE_LOCAL_VAR
                                    select row).Count();
            }

            return countPendinglist;
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost]
        //public IActionResult Index(string startDate, string endDate)
        //{
        //    string constr = @"Server=.\SQL2014;DataBase=Northwind;UID=sa;PWD=pass@123";
        //    using (SqlConnection con = new SqlConnection(constr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT OrderID, OrderDate, ShipName, ShipCity FROM Orders WHERE OrderDate BETWEEN @From AND @To", con))
        //        {
        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                cmd.Parameters.AddWithValue("@From", Convert.ToDateTime(startDate, new CultureInfo("en-GB")));
        //                cmd.Parameters.AddWithValue("@To", Convert.ToDateTime(endDate, new CultureInfo("en-GB")));
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);
        //                ViewBag.Data = dt;
        //            }
        //        }
        //    }

        //    return View();
        //}
        ////LINK: https://www.aspsnippets.com/questions/738034/Search-Filter-records-between-two-Dates-in-ASPNet-Core-MVC/
    }
}
