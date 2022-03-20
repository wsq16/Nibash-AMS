using FlatManagement.Models;
using FlatManagement.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ResolutionController : Controller
    {
        private readonly FlatDBContext _context;
        private IWebHostEnvironment webHostEnvironment;
        public IConfiguration _configuration;
        String APART_CODE_LOCAL_VAR = "";

        public ResolutionController(FlatDBContext context, IWebHostEnvironment _webHostEnvironment, IConfiguration configuration)
        {
            webHostEnvironment = _webHostEnvironment;
            _context = context;
            _configuration = configuration;
        }
        // GET: Bill
        [Authorize]
        public async Task<IActionResult> Index()
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Resolutions.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }

        // GET: Bill/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var usersList = (from users in _context.Users.Where(users => users.ApartCodeName == APART_CODE_LOCAL_VAR)
                             select new SelectListItem()
                             {
                                 Value = users.UserName.ToString(),
                                 Text = "[" + users.Flat_No + "] " + users.FirstName + " " + users.LastName
                             }).ToList();
            ViewBag.Responsibility = usersList;

            var usersListEmployee = (from userEmp in _context.Employees.Where(c => c.ApartCodeName == APART_CODE_LOCAL_VAR)
                                     select new SelectListItem()
                                     {
                                         Value = userEmp.Id.ToString(),
                                         Text = userEmp.FirstName + " " + userEmp.LastName + " (" + userEmp.Designation + ")"
                                     }).ToList();
            ViewBag.ResponsibilityEmployee = usersListEmployee;

            var agendaList = (from c in _context.Agendas.Where(c =>c.ApartCodeName == APART_CODE_LOCAL_VAR)
                              select new SelectListItem()
                              {
                                  Value = c.AgendaName.ToString(),
                                  Text = c.AgendaName
                              }).ToList();

            ViewBag.agendaList = agendaList;


            if (id == 0)
            {
                return View(new ResolutionVM());
            } else
                return View(_context.Resolutions.Find(id));
        }

        // POST: Bill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit(ResolutionVM resoObj, string startTime, string endTime, IFormFile Attachment)
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];
            var filePath = "";
            resoObj.startTime = startTime;
            resoObj.endTime = endTime;
            resoObj.ApartCodeName = APART_CODE_LOCAL_VAR;
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    if (Attachment != null && Attachment.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Files");
                        uniqueFileName = Path.GetFileName(Attachment.FileName); /*DateTime.Now.ToString("yymmssfff") + "_" +*/
                        // uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadFile.FileName);
                        filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await Attachment.CopyToAsync(fileStream);
                        }
                        resoObj.Attachment = uniqueFileName;
                    }
                } catch (Exception ex)
                {
                    resoObj.Attachment = "";
                }



                try
                {
                    if (resoObj.Attachment != "")
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Files");
                        filePath = Path.Combine(uploadsFolder, resoObj.Attachment);
                    }


                    int e_id = Convert.ToInt32(resoObj.ResponsibilityEmployee);
                    var GetEmployee = "";
                    if (e_id > 0)
                    {
                        GetEmployee = _context.Employees
                                                   .Where(p => p.Id == e_id && p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                                   .Select(p => p.Email).First();
                        resoObj.ResponsibilityEmployeeName = GetEmployee;
                    }


                    string textBody = "";
                    textBody += "Meeting Resolution On " + resoObj.AgendaName + " Point(s): " + resoObj.PointNo + " Start Date " + resoObj.StartDate + " Time " + resoObj.startTime;
                    textBody += " End Date: " + resoObj.DueDate + " Time:" + resoObj.endTime + " Note: " + resoObj.Resolution + " Closing Note: " + resoObj.ResolutionClosingNote + " Responsible Person[Flat Owner]: " + resoObj.ResponsibilityFlatOwner;
                    textBody += " Responsible Person: " + GetEmployee + " Status: " + resoObj.Status+" Thanks -"+ APART_APART_LOCAL_VAR;

                    string textSubject = resoObj.AgendaName;

                    SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                    if (resoObj.msg_SMS==true)
                    {
                        if (resoObj.to_Committee == true)
                        {   
                            SNMObj.SendNotification("SMS","Committee", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);                            
                        }

                        if (resoObj.to_Treasurer == true)
                        {
                            SNMObj.SendNotification("SMS","Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_FlatOwner == true)
                        {
                            SNMObj.SendNotification("SMS", "FlatOwner", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_All == true)
                        {
                            SNMObj.SendNotification("SMS", "", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                    }
                    if (resoObj.msg_EMAIL == true)
                    {
                        if (resoObj.to_Committee == true)
                        {
                            SNMObj.SendNotification("Email", "Committee", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_Treasurer == true)
                        {
                            SNMObj.SendNotification("Email", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_FlatOwner == true)
                        {
                            SNMObj.SendNotification("Email", "FlatOwner", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_All == true)
                        {
                            SNMObj.SendNotification("Email", "", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }
                    }
                    
                    if (resoObj.msg_BOTH == true)
                    {
                        if (resoObj.to_Committee == true)
                        {
                            SNMObj.SendNotification("SMS", "Committee", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                            SNMObj.SendNotification("Email", "Committee", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_Treasurer == true)
                        {
                            SNMObj.SendNotification("SMS", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                            SNMObj.SendNotification("Email", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_FlatOwner == true)
                        {
                            SNMObj.SendNotification("SMS", "FlatOwner", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                            SNMObj.SendNotification("Email", "FlatOwner", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }

                        if (resoObj.to_All == true)
                        {
                            SNMObj.SendNotification("SMS", "All", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                            SNMObj.SendNotification("Email", "All", APART_CODE_LOCAL_VAR, textBody, textSubject, filePath);
                        }
                    }
                    
                }
            catch (Exception ex)
            {

            }


        if (resoObj.Id == 0)
            _context.Add(resoObj);
        else
            _context.Update(resoObj);

        await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(resoObj);
        }

        [Authorize]
        public IActionResult SendMessages(int id = 0)
        {
            if (id == 0)
            {
                return View(new ResolutionVM());
            }
            else
                return View(_context.Resolutions.Find(id));
        }

        // GET: Bill/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resolutionVar = await _context.Resolutions.FindAsync(id);
            _context.Resolutions.Remove(resolutionVar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile(IFormFile FormFile)
        {
            var filename = ContentDispositionHeaderValue.Parse(FormFile.ContentDisposition).FileName.Trim('"');
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", FormFile.FileName);
            using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
            {
                await FormFile.CopyToAsync(stream);
            }
            return RedirectToAction("Index", "Home");
        }

        public List<SelectListItem> PopulateFlatOwners()
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            string constr = _configuration.GetConnectionString("DBConnectionString");

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT FirstName, Email FROM AspNetUsers WHERE ApartCodeName='" + APART_CODE_LOCAL_VAR + "'  AND IsActive='true'";
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
                                Text = sdr["FirstName"].ToString(),
                                Value = sdr["Email"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return items;
        }


        public List<SelectListItem> GetFlatOwnersMobileNum()
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            string constr = _configuration.GetConnectionString("DBConnectionString");

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT FirstName, Mobile FROM AspNetUsers WHERE ApartCodeName='" + APART_CODE_LOCAL_VAR + "'  AND IsActive='true'";
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
                                Text = sdr["FirstName"].ToString(),
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
