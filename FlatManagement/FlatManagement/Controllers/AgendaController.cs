using FlatManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class AgendaController : Controller
    {
        private readonly FlatDBContext _context;
        private IWebHostEnvironment webHostEnvironment;
        public IConfiguration _configuration;
        String APART_CODE_LOCAL_VAR = "";

        public AgendaController(IConfiguration configuration, FlatDBContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _configuration = configuration;
            webHostEnvironment = _webHostEnvironment;
            _context = context;
            

        }
        // GET: Bill
        public async Task<IActionResult> Index()
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Agendas.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }

        // GET: Bill/Create
        public IActionResult AddOrEdit(int id=0)
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            //AgendaVM agendaVMObj = new AgendaVM();
            //agendaVMObj.invitePersons = PopulateFlatOwners();

            var usersList = (from users in _context.Users.Where(p => p.Tenant == false && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                             select new SelectListItem()
                             {
                                 Value = users.UserName.ToString(),
                                 Text = users.FirstName.ToString() +" "+ users.LastName.ToString()
                             }).ToList();

            ViewBag.FlatOwner = usersList;


            //var agendaList = (from c in _context.Agendas
            //                     select new SelectListItem()
            //                     {
            //                         Value = c.AgendaName.ToString(),
            //                         Text = c.AgendaName
            //                     }).ToList();

            //ViewBag.agendaList = agendaList;



            if (id == 0)
                return View(new AgendaVM());
            else
                return View(_context.Agendas.Find(id));
        }


        // POST: Bill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(AgendaVM agenda, IFormFile FileAttachment)
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            try
            {
                agenda.ApartCodeName = APART_CODE_LOCAL_VAR;
                string uniqueFileName = null;
                if (FileAttachment != null && FileAttachment.Length > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Files");
                    uniqueFileName = DateTime.Now.ToString("yymmssfff") + "_" + Path.GetFileName(FileAttachment.FileName);
                    // uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileAttachment.CopyToAsync(fileStream);
                    }
                    agenda.Attachment = uniqueFileName;
                }


                //if (FileAttachment != null && FileAttachment.Length > 0)
                //{
                //    var path = Path.Combine(
                //           Directory.GetCurrentDirectory(), "wwwroot/Files", FileAttachment.FileName);

                //    using (var stream = new FileStream(path, FileMode.Create))
                //    {
                //        await FileAttachment.CopyToAsync(stream);
                //        agenda.Attachment = FileAttachment.FileName;
                //    }
                //}
                //else
                //{
                   
                //}
                


                //filename = Attachment.Length>0?ContentDispositionHeaderValue.Parse(Attachment.ContentDisposition).FileName.Trim('"'):"";
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", Attachment.FileName);
                //using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                //{
                //    await Attachment.CopyToAsync(stream);
                //}
                //agenda.Attachment = filename;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in file upload:" + ex.ToString());
            }


            ModelState.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    if (agenda.Id == 0)
                        _context.Add(agenda);
                    else
                        _context.Update(agenda);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {

                }
               
               
            }
            return View(agenda);
        }



        [HttpGet]
        public IActionResult Invitation(string AgendaId, string AgendaDetails, string AgendaAttachment, string AgendaDate, string AgendaName)
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            ViewBag.AgendaId = AgendaId;
            ViewBag.AgendaDetails = AgendaDetails;
            ViewBag.AgendaAttachment = AgendaAttachment;
           // var shortDT = defaultDate.ToString().Replace("12:00:00 AM", "");
            ViewBag.AgendaDate = AgendaDate.Replace("12:00:00 AM", "");
            ViewBag.AgendaName = AgendaName;

            List <SelectListItem> items = PopulateFlatOwners();
            return View(items);
            //AgendaId=@item.Id&AgendaDetails=@item.AgendaDetails&ASendInvitationgendaAttachment=@item.Attachment&AgendaDate=@item.AgendaDate"
           
        }

        [HttpPost]
        public IActionResult SendInvitation(string AgendaId, string AgendaDetails, string AgendaAttachment, string AgendaDate, string AgendaName, string[] flatOwnerList)
        {
            APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            ViewBag.Message = "Message sent to:\\n";
            List<SelectListItem> items = PopulateFlatOwners();
            var path = "";

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

                mail.Subject = "Invitation";
                mail.Body = AgendaDetails;
                mail.IsBodyHtml = true;

                if (AgendaAttachment!=null)
                {
                    path = Path.Combine(webHostEnvironment.WebRootPath, "Files", AgendaAttachment);
                    mail.Attachments.Add(new Attachment(path));
                }

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("elongatesdev@gmail.com", "aqitzohxddzutnve");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return RedirectToAction("Index");
        }

        
        public FileResult DownloadFile(string fileName)
        {
            try
            {
                //Build the File Path.
                string path = Path.Combine(this.webHostEnvironment.WebRootPath, "Files/") + fileName;

                //Read the File data into Byte Array.
                byte[] bytes = System.IO.File.ReadAllBytes(path);

                //Send the File to Download.
                return File(bytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // GET: Bill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agendas.FindAsync(id);
            _context.Agendas.Remove(agenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
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
                string query = " SELECT FirstName, Email FROM AspNetUsers WHERE ApartCodeName='"+ APART_CODE_LOCAL_VAR + "'";
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



        public JsonResult FileDelete(string filepath, string table, string field, string id)
        {
            int tid = Convert.ToInt32(id);
            string tableName = "";
            tableName = table;
            bool returnval = true;
            if (returnval)
            {

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", filepath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);

                    var serviceTbl = _context.Agendas.FirstOrDefault(u => u.Id == tid);
                    serviceTbl.Attachment = "";
                    _context.SaveChanges();
                }
            }
            return Json(returnval, new Newtonsoft.Json.JsonSerializerSettings());
        }


    }
}
