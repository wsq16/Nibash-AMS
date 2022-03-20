using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FlatManagement.Models
{
    public class MaintenanceController : Controller
    {
        public IConfiguration _configuration;
        private readonly FlatDBContext _context;
        private IWebHostEnvironment webHostEnvironment;

        public MaintenanceController(FlatDBContext context, IConfiguration configuration, IWebHostEnvironment _webHostEnvironment)
        {
            _configuration = configuration;
            _context = context;
            webHostEnvironment = _webHostEnvironment;
        }

        // GET: Flat
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Maintenances.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }



        // GET: Flat/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var contractsList = (from c in _context.EnumValues.Where(p => p.Value.Contains("CONTRACT") && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                             select new SelectListItem()
                             {
                                 Value = c.EnumText.ToString(),
                                 Text = c.EnumText
                             }).ToList();

            ViewBag.contractsList = contractsList;




            var maintenanceTypeList = (from c in _context.EnumValues.Where(p => p.Value.Contains("MAINTENANCE_TYPE") && p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                 select new SelectListItem()
                                 {
                                     Value = c.EnumText.ToString(),
                                     Text = c.EnumText
                                 }).ToList();

            ViewBag.maintenanceTypeList = maintenanceTypeList;
            



            if (id == 0)
                return View(new MaintenanceVM());
            else
                return View(_context.Maintenances.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(MaintenanceVM varObj, string contractText, string categoryRadio)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            try
            {
                
                if (categoryRadio == "1")
                {
                    varObj.contract = contractText;                    
                    varObj.category = "Ad-Hoc";
                }
                else if (categoryRadio == "2")
                {
                    varObj.category = "Schedule";
                }
                varObj.ApartCodeName = APART_CODE_LOCAL_VAR;

                ModelState.Clear(); //this will clear all value

                //varObj.category = categoryRadio.Trim().ToString()=="1"? "Ad-Hoc" : "Schedule";

                if (ModelState.IsValid)
                {
                    if (varObj.Id == 0)
                        _context.Add(varObj);
                    else
                        _context.Update(varObj);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error in saving..." + ex.ToString());
            }
            return View(varObj);
        }

        // GET: Flat/Create
        public IActionResult MaintenanceNotification(int id = 0, string category="", string amount = "", string Date = "")
        {
            ViewBag.category = category;
            ViewBag.amount = amount;
            ViewBag.Date = Date.Replace("12:00:00 AM", "");

            List<SelectListItem> items = PopulateFlatOwners();
            return View(items);
        }

        [HttpPost]
        public IActionResult SendEmailNotification(string category, string amount, string Date, string[] flatOwnerList)
        {

            ViewBag.Message = "Notification sent to:\\n";
            List<SelectListItem> items = PopulateFlatOwners();

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("jubayer.ah@gmail.com");

                foreach (SelectListItem item in items)
                {
                    if (flatOwnerList.Contains(item.Value))
                    {
                        item.Selected = true;
                        ViewBag.Message += string.Format("{0},{1}\\n", item.Text, item.Value);
                        mail.To.Add(item.Value);
                    }
                }

                mail.Subject = "Maintenance Notification";
                mail.Body = "Maintenance category"+ category+" On "+Date+" Amount "+amount;
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("../wwwroot/Files/" + AgendaAttachment));
                //mail.Attachments.Add(new Attachment(path));

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("elongatesdev@gmail.com", "aqitzohxddzutnve");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// This method is for future use
        /// </summary>
        /// <param name="category"></param>
        /// <param name="amount"></param>
        /// <param name="Date"></param>
        /// <param name="flatOwnerList"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SendSMSNotification(string category, string amount, string Date, string[] flatOwnerList)
        {
            ViewBag.Message = "SMS Notification sent to:\\n";
            List<SelectListItem> items = GetMobileNumbers();
            string Subject = "Maintenance Notification";
            string Body = "Maintenance category" + category + " On " + Date + " Amount " + amount;
            string mobileNo = "";

            foreach (SelectListItem item in items)
                {
                    if (flatOwnerList.Contains(item.Value))
                    {
                        item.Selected = true;
                        ViewBag.Message += string.Format("{0},{1}\\n", item.Text, item.Value);
                        mobileNo =item.Value;
                    }
                }

            return RedirectToAction("Index");
        }



        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valObj = await _context.Maintenances.FindAsync(id);
            _context.Maintenances.Remove(valObj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public List<SelectListItem> PopulateFlatOwners()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            string constr = _configuration.GetConnectionString("DBConnectionString");
            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT CONCAT(FirstName,' ', LastName) AS FLName, Email FROM AspNetUsers WHERE ApartCodeName='" + APART_CODE_LOCAL_VAR + "'  AND IsActive='true'";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["FLName"].ToString(),
                                Value = sdr["Email"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }


        public List<SelectListItem> GetMobileNumbers()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            string constr = _configuration.GetConnectionString("DBConnectionString");

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "SELECT CONCAT(FirstName,' ', LastName) AS FLName, Mobile FROM AspNetUsers WHERE ApartCodeName='" + APART_CODE_LOCAL_VAR + "'  AND IsActive='true'";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["FLName"].ToString(),
                                Value = sdr["Mobile"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }

    }
}
