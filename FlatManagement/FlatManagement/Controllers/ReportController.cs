using FlatManagement.Models;
using FlatManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ReportController : Controller
    {
        private readonly FlatDBContext _context;
        public ReportController(FlatDBContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
           // return View(await _context.Employees.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
            return View();
        }


        //Bills
        public async Task<IActionResult> Bills(string fromDate, string toDate)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;

            if (fromDate.Length > 0 && toDate.Length > 0)
            {
                startdate = Convert.ToDateTime(fromDate);
                enddate = Convert.ToDateTime(toDate);
                ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
                ViewBag.enddate = enddate.ToString("MM/dd/yyyy");

                return View(await _context.Bills.Where(e => e.BillStatus == "Initial" && e.DueDate >= startdate && e.DueDate <= enddate && e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
            }
            else
            {
                return View(await _context.Bills.Where(e => e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
            }

            
        }

        //Maintenance
        public async Task<IActionResult> Maintenance(string fromDate, string toDate)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;
            if (fromDate.Length > 0 && toDate.Length > 0)
            {
                startdate = Convert.ToDateTime(fromDate);
                enddate = Convert.ToDateTime(toDate);
                ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
                ViewBag.enddate = enddate.ToString("MM/dd/yyyy");

                return View(await _context.Maintenances.Where(e => e.MaintenanceDate >= startdate && e.MaintenanceDate <= enddate).ToListAsync());
            }
            else
            {
                return View(await _context.Maintenances.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
            }
        }

        //Renewal
        public async Task<IActionResult> Renewal(string fromDate, string toDate)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;
            if (fromDate.Length > 0 && toDate.Length > 0)
            {
                startdate = Convert.ToDateTime(fromDate);
                enddate = Convert.ToDateTime(toDate);
                ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
                ViewBag.enddate = enddate.ToString("MM/dd/yyyy");

                return View(await _context.Services.Where(e => e.StartDate >= startdate && e.EndDate <= enddate).ToListAsync());
            }
            else
            {
                return View(await _context.Services.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
            }
        }

        //Meeting
        public async Task<IActionResult> Meeting(string fromDate, string toDate, string status)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;
            if (fromDate.Length > 0 && toDate.Length > 0)
            {
                startdate = Convert.ToDateTime(fromDate);
                enddate = Convert.ToDateTime(toDate);
                ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
                ViewBag.enddate = enddate.ToString("MM/dd/yyyy");

                var query = from m in _context.Resolutions
                            select m;
                query = query.Where(s => s.DueDate >= startdate && s.DueDate <= enddate);
                if (!String.IsNullOrEmpty(status))
                {
                    query = query.Where(s => s.Status!.Contains(status));
                }


                return View(await query.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
                // return View(await _context.Resolutions.Where(e => e.DueDate >= startdate && e.DueDate <= enddate).ToListAsync()); // && e.Status== status
            }
            else
            {
                return View(await _context.Resolutions.Where(e => e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
            }
            
        }


        public ActionResult AllCollection(string fromDate, string toDate)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var collectionList = _context.CommonFunds.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToList();
            var billList = _context.Bills.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToList();
            
            //ViewBag.Message = "Welcome to my demo!";
            CollectionViewModel mymodel = new CollectionViewModel();
                DateTime startdate = DateTime.Now;
                DateTime enddate = DateTime.Now;
                if (fromDate.Length > 0 && toDate.Length > 0)
                {
                    startdate = Convert.ToDateTime(fromDate);
                    enddate = Convert.ToDateTime(toDate);
                    ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
                    ViewBag.enddate = enddate.ToString("MM/dd/yyyy");
                    collectionList = _context.CommonFunds.Where(e => e.CollectionDate >= startdate && e.CollectionDate <= enddate && e.ApartCodeName == APART_CODE_LOCAL_VAR).ToList();
                    billList = _context.Bills.Where(e => e.DueDate >= startdate && e.DueDate <= enddate && e.BillStatus == "Paid" && e.ApartCodeName == APART_CODE_LOCAL_VAR).ToList();
                }

                mymodel.CommonFundVObj = collectionList;
                mymodel.BillObj = billList;
                return View(mymodel);
            
        }



        //Collection
        //public async Task<IActionResult> Collection(string fromDate, string toDate)
        //{
        //    DateTime startdate = DateTime.Now;
        //    DateTime enddate = DateTime.Now;
        //    if (fromDate.Length > 0 && toDate.Length > 0)
        //    {
        //        startdate = Convert.ToDateTime(fromDate);
        //        enddate = Convert.ToDateTime(toDate);
        //        ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
        //        ViewBag.enddate = enddate.ToString("MM/dd/yyyy");

        //        return View(await _context.CommonFunds.Where(e => e.CollectionDate >= startdate && e.CollectionDate <= enddate).ToListAsync());
        //    }
        //    else
        //    {
        //        return View(await _context.CommonFunds.ToListAsync());
        //    }
            
        //}

        //Pending Task
        public async Task<IActionResult> PendingTask(string fromDate, string toDate, string status)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            DateTime startdate = DateTime.Now;
            DateTime enddate = DateTime.Now;
            if (fromDate.Length > 0 && toDate.Length > 0)
            {
                startdate = Convert.ToDateTime(fromDate);
                enddate = Convert.ToDateTime(toDate);
                ViewBag.startdate = startdate.ToString("MM/dd/yyyy");
                ViewBag.enddate = enddate.ToString("MM/dd/yyyy");


                var query = from m in _context.Resolutions
                            select m;
                query = query.Where(s => s.DueDate >= startdate && s.DueDate <= enddate && s.ApartCodeName== APART_CODE_LOCAL_VAR);
                if (!String.IsNullOrEmpty(status))
                {
                    query = query.Where(s => s.Status!.Contains(status) && s.ApartCodeName== APART_CODE_LOCAL_VAR);
                }


                //return View(await movies.ToListAsync());
                //var query = _context.Resolutions;
                //if (status != null && status!="")
                //{
                //    query = query.Where(e => e.Status == null && filters.Market.Contains(x.Market));
                //}
                //if (!String.IsNullOrEmpty(status))
                //{
                //    query = query.Where(s => s.Status!.Contains(searchString));
                //}


                //var filteredData = query.ToList();



                //return View(await _context.Resolutions.Where(e => e.DueDate >= startdate && e.DueDate <= enddate).ToListAsync()); //&& e.Status== status
                //return View(await _context.Resolutions.ToListAsync());
                return View(await query.ToListAsync());
            }
            else
            {
                return View(await _context.Resolutions.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
            }
            
        }

    }
}
