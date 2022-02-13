using FlatManagement.Models;
using FlatManagement.Utility;
using FlatManagement.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class TransactionController : Controller
    {
        private readonly FlatDBContext _context;
        private IWebHostEnvironment webHostEnvironment;
        public IConfiguration _configuration;
        public TransactionController(FlatDBContext context, IWebHostEnvironment _webHostEnvironment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            webHostEnvironment = _webHostEnvironment;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Add()
        {
            try
            {
                int num1 = Convert.ToInt32(HttpContext.Request.Form["Text1"].ToString());
                int num2 = Convert.ToInt32(HttpContext.Request.Form["Text2"].ToString());
                ViewBag.Result = (num1 + num2).ToString();
            }
            catch (Exception)
            {
                ViewBag.Result = "Wrong Input Provided.";
            }
            return View("CalPage");
        }



        // GET: Flat
        public async Task<IActionResult> Index(string? stStatus, int? trn, string billId="")
        {
            if (trn == 1)
            {
                if (billId.Length > 0)
                {
                    string vId = "(" + billId + ")";
                    string connectionString = _configuration.GetConnectionString("DBConnectionString");
                    SqlConnection sqlConn = new SqlConnection(connectionString);
                    string query = "UPDATE Transactions SET TransactionStep = 'Paid', Claim= (CASE WHEN Claim=0 THEN 1 WHEN Claim=2 THEN 3 ELSE 9 END) WHERE Id = " + vId;
                    SqlCommand cmd = new SqlCommand(query, sqlConn);
                    sqlConn.Open();
                    int i = cmd.ExecuteNonQuery();
                    sqlConn.Close();
                }
            }


            if (User.IsInRole("Admin")||User.IsInRole("Supervisor"))
            {
                return View(await _context.Transactions.ToListAsync());
            }else if(User.IsInRole("Committee"))
            {
                return View(await _context.Transactions.Where(e => e.TransactionStep=="Submitted" || e.Claim == 2).ToListAsync());
            }
            else if (User.IsInRole("Treasurer"))
            {
                //return View(await _context.Transactions.ToListAsync());
                return View(await _context.Transactions.Where(e => e.TransactionStep == "COM"|| e.TransactionStep == "TRE").ToListAsync());
            }
            else if (User.IsInRole("FlatOwner"))
            {
                string userName = User.Identity.Name;
                var flatNo = _context.Users
                                               .Where(p => p.UserName == userName)
                                               .Select(p => p.Flat_No).First();

                return View(await _context.Transactions.Where(e => e.FlatNo == flatNo).ToListAsync());
            }
            else
            {
                return View(await _context.Transactions.ToListAsync());
            }
        }



        [HttpGet]
        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0, string singleFlatNo="")
        {
            //List<FlatVM> cl = new List<FlatVM>();
            //cl = (from c in _context.Flats select c).ToList();
            //var query = _context.var result = from c in Customer where !subselect.Contains(c.CusId) select c;.Where(x => x.Id == id && x.Transactions.Any(z => x.FlatNo == x.FlatNo)).ToList();
            //cl = (from c in _context.Flats select c).ToList();
            //cl.Insert(0, new FlatVM { FlatValue = "", FlatNo = "--Select Flat--" });
            //ViewBag.FlatNo = cl;

            //var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
            //                 select new SelectListItem()
            //                 {
            //                     Value = users.Flat_No.ToString()+" "+users.FirstName+ " "+users.LastName,
            //                     Text = users.Flat_No
            //                 }).Distinct().ToList();

            //ViewBag.FlatNo = usersList;
            string userName = User.Identity.Name;
            var flatNo = _context.Users
                                           .Where(p => p.UserName == userName)
                                           .Select(p => p.Flat_No).First();
            if(flatNo == singleFlatNo)
            {
                ViewBag.boolFlat = "1";
            }
            else
            {
                ViewBag.boolFlat = "0";
            }
            



            var getFlatLst = (from u in _context.Users
                              join f in _context.FlatConfigs
                                     on u.Flat_No equals f.FlatNo
                              select new SelectListItem()
                              {
                                  Value = f.FlatNo.ToString(),
                                  Text = f.FlatNo + " (" + u.FirstName + " " + u.LastName+")"
                              }).Distinct().ToList();
            ViewBag.FlatNo = getFlatLst;


            if (id == 0)
            {
                TransactionVM vm = new TransactionVM();
                vm.Date = DateTime.Now;

                ViewData["readonly"] = "";
                return View(vm);
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Transactions.Find(id));
            }
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(TransactionVM transactionVM, string TransactionType, string TransactionCategory, IFormFile FormFile)
        {
            bool flagOwnerAdmin = false;
            string userName = User.Identity.Name;
            var flatNo = _context.Users
                                           .Where(p => p.UserName == userName)
                                           .Select(p => p.Flat_No).First();
            if (flatNo == transactionVM.FlatNo)
            {
                flagOwnerAdmin = true;
                
            }
            


            if (FormFile == null || FormFile.Length == 0)
                {
                   
                }
                else
                {
                    var path = Path.Combine(
                           Directory.GetCurrentDirectory(), "wwwroot/Files", FormFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await FormFile.CopyToAsync(stream);
                        transactionVM.ReceiptFile = FormFile.FileName;
                    }
                }

                if(User.IsInRole("Admin") || User.IsInRole("Supervisor"))
                {
                    if (transactionVM.Id == 0)
                    {

                        transactionVM.PreparedBy = User.Identity.IsAuthenticated ? HttpContext.User.Identity.Name : "-";
                        
                    }

                    
                
                
                if (string.IsNullOrWhiteSpace(transactionVM.FlatNo))
                    {
                        transactionVM.FlatNo = "";
                    }
                    else
                    {
                        var flatVar = _context.Users
                                                .Where(p => p.Flat_No == transactionVM.FlatNo.ToString())
                                                .Select(p => p.FlatOwner).First();
                        transactionVM.FlatOwner = flatVar;

                    }
                }

            if ((transactionVM.TransactionStep == "Submitted") && (User.IsInRole("Supervisor") || User.IsInRole("Admin") ))
            {
                transactionVM.TransactionStep = "COM";
                transactionVM.NextStatus = "TRE";
            }
            

            if (((transactionVM.TransactionStep == "Submitted")|| (transactionVM.TransactionStep == "COM")) && User.IsInRole("Committee"))
                {
                        if(transactionVM.TransactionStep == "COM")
                        {
                            transactionVM.TransactionStep = "TRE";
                            transactionVM.NextStatus = "Settlement";
                        }
                        else
                        {
                            transactionVM.TransactionStep = "COM";
                            transactionVM.NextStatus = "TRE";
                        }
                    
                }

                if ((transactionVM.TransactionStep == "COM") && User.IsInRole("Treasurer"))
                {
                    transactionVM.TransactionStep = "TRE";
                    transactionVM.NextStatus = "Settlement";
                }

                if (User.IsInRole("FlatOwner")|| User.IsInRole("Supervisor") || User.IsInRole("Admin"))
                {
                    if ((User.IsInRole("Admin")) && (flagOwnerAdmin == false))
                    {
                        if(transactionVM.Claim == 2)
                        {
                            transactionVM.TransactionStep = "COM";
                            transactionVM.NextStatus = "TRE";
                        }
                        else
                        {
                            transactionVM.TransactionStep = "Submitted";
                            transactionVM.NextStatus = "COM";
                        }
                    } 
                    else if((User.IsInRole("Admin")) && (flagOwnerAdmin == true))
                    {
                        transactionVM.Claim = 2;
                        transactionVM.TransactionStep = "Submitted";
                        transactionVM.NextStatus = "COM";
                    }
                    else
                    {
                        transactionVM.TransactionStep = "Submitted";
                        transactionVM.NextStatus = "COM";
                    }
                }


            //if needed the uncomment
            //if (User.IsInRole("FlatOwner"))
            //{
            //    transactionVM.FlatOwner = transactionVM.PreparedBy;

            //    var flatVar = _context.Users
            //    .Where(p => p.UserName == transactionVM.PreparedBy.ToString())
            //    .Select(p => p.Flat_No).First();

            //    transactionVM.FlatNo = flatVar; 
            //    transactionVM.PaidBy = transactionVM.PreparedBy;
            //}
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                    if (transactionVM.Id == 0)
                        _context.Add(transactionVM);
                    else
                        _context.Update(transactionVM);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
             
            return View(transactionVM);
        }

        [HttpGet]    
        // GET: Flat/Create
            public IActionResult Approve(int id = 0)
        {
            //List<FlatVM> cl = new List<FlatVM>();
            //cl = (from c in _context.Flats select c).ToList();
            //var query = _context.var result = from c in Customer where !subselect.Contains(c.CusId) select c;.Where(x => x.Id == id && x.Transactions.Any(z => x.FlatNo == x.FlatNo)).ToList();
            //cl = (from c in _context.Flats select c).ToList();
            //cl.Insert(0, new FlatVM { FlatValue = "", FlatNo = "--Select Flat--" });
            //ViewBag.FlatNo = cl;

            var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
                             select new SelectListItem()
                             {
                                 Value = users.Flat_No.ToString(),
                                 Text = users.Flat_No
                             }).Distinct().ToList();

            ViewBag.FlatNo = usersList;


            if (id == 0)
            {
                ViewData["readonly"] = "";
                return View(new TransactionVM());
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Transactions.Find(id));
            }
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve([Bind("Id,Purpose,PaidBy,Amount,Date,PreparedBy,ReceiptNumber,FlatNo,ReceiptFile,TransactionCategory,TransactionType,TransactionStep,FlatOwner,NextStatus,CurrentStatus ")] TransactionVM transactionVM, IFormFile FormFile)
        {
            if (ModelState.IsValid)
            {
                if (FormFile == null || FormFile.Length == 0)
                {
                    //return Content("File not selected");
                }
                else
                {
                    var filename = ContentDispositionHeaderValue.Parse(FormFile.ContentDisposition).FileName.Trim('"');
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", FormFile.FileName);
                    using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await FormFile.CopyToAsync(stream);
                    }
                    transactionVM.ReceiptFile = FormFile.FileName;
                }

                if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
                {
                    if (string.IsNullOrWhiteSpace(transactionVM.FlatNo))
                    {
                        transactionVM.FlatNo = "";
                    }
                    else
                    {
                        var flatVar = _context.Users
                                                .Where(p => p.Flat_No == transactionVM.FlatNo.ToString())
                                                .Select(p => p.FlatOwner).First();
                        transactionVM.FlatOwner = flatVar;

                    }
                }

                if ((transactionVM.TransactionStep == "Submitted") && User.IsInRole("Committee"))
                {
                    transactionVM.TransactionStep = "COM";
                    transactionVM.NextStatus = "TRE";
                }

                if ((transactionVM.TransactionStep == "COM") && User.IsInRole("Treasurer"))
                {
                    transactionVM.TransactionStep = "TRE";
                    transactionVM.NextStatus = "Settlement";
                }


                if ((transactionVM.Id == 0) && (User.IsInRole("FlatOwner") || User.IsInRole("Supervisor")))
                {
                    transactionVM.TransactionStep = "Submitted";
                    transactionVM.NextStatus = "COM";
                }



                if (User.IsInRole("FlatOwner"))
                {
                    transactionVM.FlatOwner = transactionVM.PreparedBy;

                    var flatVar = _context.Users
                    .Where(p => p.UserName == transactionVM.PreparedBy.ToString())
                    .Select(p => p.Flat_No).First();

                    transactionVM.FlatNo = flatVar;
                    transactionVM.PaidBy = transactionVM.PreparedBy;
                }


                if (transactionVM.Id == 0)
                    _context.Add(transactionVM);
                else
                    _context.Update(transactionVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactionVM);
        }


        [HttpGet]
        // GET: Flat/Create
        public IActionResult ApproveSplit(int id = 0)
        {
            var billType = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_TYPE"))
                            select new SelectListItem()
                            {
                                Value = b.EnumText.ToString(),
                                Text = b.EnumText
                            }).ToList();

            ViewBag.billType = billType;

            var count = _context.Users.Count(p => p.Flat_No != null);
            if (id > 0)
            {
                var totalAmount = _context.Transactions.Where(p => p.Id == id).Select(p => p.Amount).FirstOrDefault();
                var splitAmount = totalAmount / count;
                ViewBag.splitAmountView = splitAmount;
            }
            var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
                             select new SelectListItem()
                             {
                                 Value = users.Flat_No.ToString(),
                                 Text = users.Flat_No
                             }).Distinct().ToList();
            ViewBag.FlatNo = usersList;


            if (id == 0)
            {
                ViewData["readonly"] = "";
                return View(new TransactionVM());
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Transactions.Find(id));
            }
        }


        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveSplit(TransactionVM transactionVM, IFormFile FormFile)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                var count = _context.Users.Count(p => p.Flat_No != null);

                var totalAmount = transactionVM.Amount;
                var splitAmount = totalAmount / count;

                if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
                {
                    var flatVar = _context.Users
                                                .Where(p => p.Flat_No != null)
                                                .Select(p => new { p.Email, p.Flat_No }).ToList();
                    foreach (var item in flatVar)
                    {
                        BillVM billVMObj = new BillVM();
                        var e_mail = item.Email;
                        var e_FlatNo = item.Flat_No;
                        if (e_FlatNo == transactionVM.FlatNo)
                        {
                            billVMObj.BillStatus = "Paid";
                        }
                        else
                        {
                            billVMObj.BillStatus = "Initial";
                        }
                        billVMObj.Amount = splitAmount;
                        billVMObj.EntryDate = DateTime.Now;
                        billVMObj.DueDate = transactionVM.BillDueDate;
                        billVMObj.BillDate = transactionVM.BillDate;
                        billVMObj.BillFor = transactionVM.FlatNo;
                        billVMObj.BillType = transactionVM.BillType;
                        billVMObj.FlatNo = e_FlatNo;
                        billVMObj.PreparedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);                        
                        _context.Bills.Add(billVMObj);

                        string emailSubject = "A Bill Created";
                        string smsBody = "A Bill Initiated. Please login to system and pay the bill on time.";
                        BroadCast nB = new BroadCast();
                        nB.sendBroadCastMail("Email", e_mail, smsBody, emailSubject);
                    }
                    transactionVM.TransactionStep = "Paid";
                }
                else if(User.IsInRole("Treasurer"))
                {
                    transactionVM.Claim = 4;
                }

                if (transactionVM.Id == 0)
                    _context.Add(transactionVM);
                else
                    _context.Update(transactionVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactionVM);
        }

        // GET: Flat
        public async Task<IActionResult> GetSplitListIndex()
        {
            var countTotalUser = _context.Users.Count(p => p.Flat_No != null);
            if (countTotalUser > 0)
            { 
                ViewBag.countTotalUserView = countTotalUser;
            }

            if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
            {
                return View(await _context.Transactions.Where(e => e.Claim == 4).ToListAsync());
            }
            else
            {
                return View(await _context.Transactions.ToListAsync());
            }
        }


        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vals = await _context.Transactions.FindAsync(id);
            vals.TransactionStep = "Rejected";
            vals.NextStatus = "Rejected";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Flat/Create
        public IActionResult IndividualClaim(int id = 0)
        {

            var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
                             select new SelectListItem()
                             {
                                 Value = users.Flat_No.ToString(),
                                 Text = users.Flat_No
                             }).Distinct().ToList();

            ViewBag.FlatNo = usersList;


            if (id == 0)
            {
                ViewData["readonly"] = "";
                return View(new TransactionVM());
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Transactions.Find(id));
            }
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndividualClaim(TransactionVM transactionVM, IFormFile FormFile)
        {
            if (transactionVM.BillAmount == 0)
                transactionVM.BillAmount = 0;

            if (transactionVM.TransactionType == 0)
                transactionVM.TransactionType = 0;

                    transactionVM.Claim = 2;
                    transactionVM.TransactionStep = "Submitted";
                    transactionVM.NextStatus = "COM";

            if (FormFile == null || FormFile.Length == 0)
            {
                //return Content("File not selected");
            }
            else
            {
                var filename = ContentDispositionHeaderValue.Parse(FormFile.ContentDisposition).FileName.Trim('"');
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", FormFile.FileName);
                using (System.IO.Stream stream = new FileStream(path, FileMode.Create))
                {
                    await FormFile.CopyToAsync(stream);
                }
                transactionVM.ReceiptFile = FormFile.FileName;
            }

            if (transactionVM.Id == 0)
                    _context.Add(transactionVM);
                else
                    _context.Update(transactionVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            
        }





        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
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
            }catch(Exception ex)
            {
                return null;
            }
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vals = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(vals);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




    }
}
