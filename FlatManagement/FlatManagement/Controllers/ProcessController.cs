using FlatManagement.Models;
using FlatManagement.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ProcessController : Controller
    {
        private readonly FlatDBContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        public IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProcessController(FlatDBContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this._userManager = userManager;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Flat

        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Processes.ToListAsync());
        //}


        // GET: Flat
        [Authorize]
        public async Task<IActionResult> Index(string? stStatus, int? trn, string billId = "")
        {
            string userName = User.Identity.Name;
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            if (trn == 1)
            {
                if (billId.Length > 0)
                {
                    string vId = "(" + billId + ")";
                    string connectionString = _configuration.GetConnectionString("DBConnectionString");
                    SqlConnection sqlConn = new SqlConnection(connectionString);
                    string query = "UPDATE Processes SET PaymentStatus = 'Paid'  WHERE Id = " + vId;  // , Claim=1 (CASE WHEN Claim=0 THEN 1 WHEN Claim=2 THEN 3 ELSE 9 END)
                    SqlCommand cmd = new SqlCommand(query, sqlConn);
                    sqlConn.Open();
                    int i = cmd.ExecuteNonQuery();
                    sqlConn.Close();

                    if (billId.Length > 0)
                    {
                        int bId = Convert.ToInt32(billId);
                        var GetProcessInfor = _context.Processes
                                               .Where(p => p.Id == bId)
                                               .Select(p => new { p.Flow, p.Amount, p.FlatNo, p.Purpose }).ToList();

                        foreach (var item in GetProcessInfor)
                        {
                            string textBody = "Your "+ item.Flow + " amount: "+item.Amount+" on "+item.Purpose+ " has been paid. Thank you --By Treasurer";
                            string textSubject = "Payment Completed";
                            SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                            SNMObj.SendNotification("All","Committee", APART_CODE_LOCAL_VAR, textBody, textSubject);
                            SNMObj.SendNotification("All","Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject);
                        }
                            
                        

                    }

                    


                }
            }
            
            var OwnflatNo = _context.Users
                                           .Where(p => p.UserName == userName&&p.ApartCodeName== APART_CODE_LOCAL_VAR)
                                           .Select(p => p.Flat_No).First();
            ViewBag.OwnflatNo = OwnflatNo;

            if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
            {
                //.OrderByDynamis(student => student.Name, ListSortDirection.Ascending);
                return View(await _context.Processes.Where(e => e.DeleteFlag == false && e.ApartCodeName == APART_CODE_LOCAL_VAR).OrderByDescending(e => e.Id).ToListAsync());
            }
            else if (User.IsInRole("Committee"))
            {
                return View(await _context.Processes.Where(e => e.DeleteFlag == false && e.ApartCodeName == APART_CODE_LOCAL_VAR).OrderByDescending(e => e.Id).ToListAsync());
                //return View(await _context.Processes.Where(e => e.TransactionStep == "Submitted" || e.Claim == 2).ToListAsync());
            }
            else if (User.IsInRole("Treasurer"))
            {
                return View(await _context.Processes.Where(e => e.DeleteFlag == false && e.ApartCodeName == APART_CODE_LOCAL_VAR).OrderByDescending(e => e.Id).ToListAsync());
                //return View(await _context.Processes.Where(e => e.TransactionStep == "COM" || e.TransactionStep == "TRE").ToListAsync());
            }
            else if (User.IsInRole("FlatOwner"))
            {
                //string userName = User.Identity.Name;
                var flatNo = _context.Users
                                               .Where(p => p.UserName == userName)
                                               .Select(p => p.Flat_No).First();

                return View(await _context.Processes.Where(e => e.FlatNo == flatNo && e.DeleteFlag == false&&e.ApartCodeName== APART_CODE_LOCAL_VAR).OrderByDescending(e => e.Id).ToListAsync());
            }
            else
            {
                return View(await _context.Processes.Where(e=> e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
            }
        }





        // GET: Flat/Create
        [HttpGet]
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var getFlatLst = (from u in _context.Users
                              join f in _context.FlatConfigs
                                     on u.Flat_No equals f.FlatNo where u.ApartCodeName== APART_CODE_LOCAL_VAR
                              select new SelectListItem()
                              {
                                  Value = f.FlatNo.ToString(),
                                  Text = f.FlatNo + " (" + u.FirstName + " " + u.LastName + ")"
                              }).Distinct().ToList();
            ViewBag.FlatNo = getFlatLst;


            if (id == 0)
                return View(new ProcessVM());
            else
                return View(_context.Processes.Find(id));
        }


        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(ProcessVM processVM)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];

            processVM.ApartCodeName = APART_CODE_LOCAL_VAR;
            try
            {
                int transType = Convert.ToInt32(processVM.TransactionCategory);
                processVM.Flow = processVM.FlowTypes.ToString();

                if(transType == 1)
                {
                    string GetRole = GetFlow(processVM.Flow, processVM.Amount);
                    if (GetRole == "Committee")
                    {
                        processVM.PaymentStatus = "In-Progress";
                        string textBody = "Request for payment Approval for expense on " + processVM.Purpose + " Amount:" + processVM.Amount + " Date: " + processVM.TransDate+" Thanks -"+ APART_APART_LOCAL_VAR;
                        string textSubject = "A bill initiated from common fund. ";
                        SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                        SNMObj.SendNotification("All",GetRole, APART_CODE_LOCAL_VAR, textBody, textSubject);
                    }
                    else
                    {
                        processVM.PaymentStatus = "Initial";

                        string textBody = "A payment expense on " + processVM.Purpose + " Amount:" + processVM.Amount + " Date: " + processVM.TransDate+" Thanks -"+ APART_APART_LOCAL_VAR;
                        string textSubject = "A bill initiated from common fund.";
                        SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                        SNMObj.SendNotification("All", "Committee", APART_CODE_LOCAL_VAR, textBody, textSubject);
                        SNMObj.SendNotification("All", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject);
                    }
                    string userName = User.Identity.Name;
                    // Resolve the user via their email
                    var user = await _userManager.FindByEmailAsync(userName);
                    // Get the roles for the user
                    var roles = await _userManager.GetRolesAsync(user);


                    processVM.curr_ApprovedByRole = roles[0].ToString();
                    processVM.Next_ApprovedByRole = GetRole;
                }
                else if (transType == 2)
                {
                   // processVM.PaymentStatus = "Initial";

                    var flat_No = processVM.FlatNo;
                    var GetOwner = _context.Users
                                               .Where(p => p.Flat_No == flat_No && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                                               .Select(p => p.FlatOwner).First();

                    processVM.PaymentStatus = "Pending";
                    processVM.ClaimRefId = 0;
                    processVM.Claim = 1;

                    processVM.curr_ApprovedByRole = "Supervisor";
                    processVM.Next_ApprovedByRole = "Flat Owner";
                    processVM.FlatOwner = GetOwner;

                    string textBody = "Payment has been made for flat owner of " + processVM.FlatNo + " for expense on- " + processVM.Purpose + ". Amount:" + processVM.Amount + " Date:" + processVM.TransDate+" Thanks -"+ APART_APART_LOCAL_VAR;
                    string textSubject = "Payment initiated";

                    SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                    SNMObj.SendNotification("All","Committee", APART_CODE_LOCAL_VAR, textBody, textSubject);
                    SNMObj.SendNotification("All", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject);
                    SNMObj.OwnerNotification(processVM.FlatNo, APART_CODE_LOCAL_VAR, textBody, textSubject);
                    // Gen mail to committee, flatowner, treasurer

                }





            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (processVM.Id == 0)
                    _context.Add(processVM);
                else
                    _context.Update(processVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processVM);
        }

        [HttpGet]
        // GET: Flat/Create
        public IActionResult IndividualClaim(int id = 0)
        {
            if (id == 0)
                return View(new ProcessVM());
            else
                return View(_context.Processes.Find(id));
        }


        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndividualClaim(ProcessVM processVM, IFormFile FormFile)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];
           

            ProcessVM newProcess = null;
            try
            {
                newProcess = new ProcessVM();
                if (FormFile.Length > 0)
                {
                    string uniqueFileName = null;
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "DocFiles");
                    uniqueFileName = Path.GetFileName(FormFile.FileName); /*DateTime.Now.ToString("yymmssfff") + "_" +*/
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        newProcess.ReceiptFile = uniqueFileName;
                        await FormFile.CopyToAsync(fileStream);
                    }
                }

                
                newProcess.Flow = "Claim";
                newProcess.Amount = processVM.Amount;
                newProcess.Purpose = processVM.Purpose;
                newProcess.PaidBy = processVM.PaidBy;
                newProcess.Amount = processVM.Amount;
                newProcess.EntryDate = DateTime.Now;
                newProcess.TransDate = processVM.TransDate;
                newProcess.PreparedBy = User.Identity.Name; 
                newProcess.ReceiptNumber = processVM.ReceiptNumber;
                newProcess.FlatNo = processVM.FlatNo;
                newProcess.FlatOwner = processVM.FlatOwner;
                newProcess.TransactionCategory = "2";
                newProcess.Claim = 2;
                newProcess.ClaimRefId = processVM.Id;
                newProcess.ApartCodeName = APART_CODE_LOCAL_VAR;
                //newProcess.ReceiptFile = processVM.ReceiptFile;



                string GetRole = GetFlow("Claim", processVM.Amount);

                if (GetRole == "Committee")
                {
                    newProcess.PaymentStatus = "In-Progress";
                    newProcess.curr_ApprovedByRole = "Committee";
                    newProcess.Next_ApprovedByRole = "Treasurer";
                }
                else if(GetRole == "Admin"|| GetRole == "Supervisor")
                {
                    newProcess.PaymentStatus = "Initial";
                    newProcess.curr_ApprovedByRole = "Supervisor";
                    newProcess.Next_ApprovedByRole = "Treasurer";
                }
                else if (GetRole == "Treasurer")
                {
                    newProcess.PaymentStatus = "Initial";
                    newProcess.curr_ApprovedByRole = "Apply Claim";
                    newProcess.Next_ApprovedByRole = "Treasurer";
                }

                if (GetRole == "")
                {
                    ViewBag.ErrorMessage = "Please Setup Approval Limit.";
                }
                //string userName = User.Identity.Name;
                //// Resolve the user via their email
                //var user = await _userManager.FindByEmailAsync(userName);
                //// Get the roles for the user
                //var roles = await _userManager.GetRolesAsync(user);




                string textBody = "Request for claim approval for flat owner " + processVM.FlatNo + " Amount:" + processVM.Amount + " Date: " + processVM.TransDate+" Thanks -"+ APART_APART_LOCAL_VAR;
                string textSubject = "Claim Request";
                SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                SNMObj.SendNotification("All", "Committee", APART_CODE_LOCAL_VAR, textBody, textSubject);
                SNMObj.SendNotification("All", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject);
                SNMObj.OwnerNotification(processVM.FlatNo, APART_CODE_LOCAL_VAR, textBody, textSubject);

                        //var flatNo = processVM.FlatNo;
                        //var GetOwner = _context.Users
                        //                           .Where(p => p.Flat_No == flatNo)
                        //                           .Select(p => p.FlatOwner).First();

                        //processVM.FlatOwner = GetOwner;
                        _context.Add(newProcess);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                processVM.PaymentStatus = "Claimed";
                processVM.Claim = 2;
                if (processVM.Id == 0)
                    _context.Add(processVM);
                else
                    _context.Update(processVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newProcess);
        }



        [HttpGet]
        // GET: Flat/Create
        public IActionResult CommitteeApproval(int id = 0)
        {
            if (id == 0)
                return View(new ProcessVM());
            else
                return View(_context.Processes.Find(id));
        }


        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CommitteeApproval(ProcessVM processVM)
        {
            try
            {
                var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
                var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];
                processVM.ApartCodeName = APART_CODE_LOCAL_VAR;
                //processVM.Flow = processVM.FlowTypes.ToString();
                //string GetRole = GetFlow(processVM.Flow, processVM.Amount);
                //if (GetRole == "Committee")
                //{
                //    processVM.PaymentStatus = "In-Progress";
                //}
                //else
                //{
                //    processVM.PaymentStatus = "Initial";
                //}
                string userName = User.Identity.Name;
                // Resolve the user via their email
                var user = await _userManager.FindByEmailAsync(userName);
                // Get the roles for the user
                var roles = await _userManager.GetRolesAsync(user);


                processVM.curr_ApprovedByRole = roles[0].ToString();
                processVM.Next_ApprovedByRole = "Treasurer";


                string textBody = "Request for payment Approval for expense on " + processVM.Purpose + " Amount:" + processVM.Amount + " Date: " + processVM.TransDate+" Thanks -"+ APART_APART_LOCAL_VAR;
                string textSubject = "Payment Approved";
                SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                //SNMObj.SendNotification("Committee", textBody, textSubject);
                SNMObj.SendNotification("All", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (processVM.Id == 0)
                    _context.Add(processVM);
                else
                    _context.Update(processVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processVM);
        }


        [HttpGet]
        [Authorize]
        // GET: Flat/Create
        public IActionResult ApproveSplit(int id = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
           
            var billType = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_TYPE") && b.ApartCodeName == APART_CODE_LOCAL_VAR)
                            select new SelectListItem()
                            {
                                Value = b.EnumText.ToString(),
                                Text = b.EnumText
                            }).ToList();

            ViewBag.billType = billType;

            var count = _context.Users.Count(p => p.Flat_No != null);
            if (id > 0)
            {
                var totalAmount = _context.Processes.Where(p => p.Id == id && p.ApartCodeName== APART_CODE_LOCAL_VAR).Select(p => p.Amount).FirstOrDefault();
                var splitAmount = totalAmount / count;
                ViewBag.splitAmountView = splitAmount;
            }
            //var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
            //                 select new SelectListItem()
            //                 {
            //                     Value = users.Flat_No.ToString(),
            //                     Text = users.Flat_No
            //                 }).Distinct().ToList();
            //ViewBag.FlatNo = usersList;

            if (id == 0)
            {
                ViewData["readonly"] = "";
                return View(new ProcessVM());
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Processes.Find(id));
            }
        }


        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveSplit(ProcessVM processVM, IFormFile FormFile)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                var count = _context.Users.Count(p => p.Flat_No != null);

                var totalAmount = processVM.Amount;
                var splitAmount = totalAmount / count;
                processVM.SplitAmt = splitAmount;
                if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
                {
                    var flatVar = _context.Users
                                                .Where(p => p.Flat_No != null && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                                                .Select(p => new {p.Mobile, p.Email, p.Flat_No }).ToList();

                    string textBody = "Just to notify that the claim " + processVM.Amount + " on " + processVM.Purpose + " will be paid after collectting through our monthly bill. - By Treaurer "+ APART_APART_LOCAL_VAR;
                    string textSubject = "Claim Request";
                    SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                    //SNMObj.SendNotification("Committee", textBody, textSubject);
                    SNMObj.SendNotification("All", "Treasurer", APART_CODE_LOCAL_VAR, textBody, textSubject);
                    SNMObj.OwnerNotification(processVM.FlatNo, APART_CODE_LOCAL_VAR, textBody, textSubject);
                    foreach (var item in flatVar)
                    {
                        BillVM billVMObj = new BillVM();
                        var e_mail = item.Email;
                        var e_FlatNo = item.Flat_No;
                        var e_mobile = item.Mobile;
                        //if (e_FlatNo == processVM.FlatNo)
                        //{
                        //    billVMObj.BillStatus = "Paid";
                        //}
                        //else
                        //{
                        billVMObj.BillStatus = "Initial";
                        //}
                        billVMObj.Amount = splitAmount;
                        billVMObj.EntryDate = DateTime.Now;
                        billVMObj.DueDate = processVM.BillDueDate;
                        billVMObj.BillDate = processVM.BillDate;
                        billVMObj.BillFor = processVM.FlatNo;
                        billVMObj.BillType = processVM.BillType;
                        billVMObj.FlatNo = e_FlatNo;
                        billVMObj.PreparedBy = User.Identity.Name;
                        billVMObj.ApartCodeName = APART_CODE_LOCAL_VAR;
                        _context.Bills.Add(billVMObj);

                        string emailSubject = "A Bill Created";
                        string smsBody = "A Bill Initiated. Please login to system and pay the bill on time. -"+ APART_APART_LOCAL_VAR;

                        //EMAIL HERE
                        //BroadCast nB = new BroadCast();
                        //nB.sendBroadCastMail("Email", e_mail, smsBody, emailSubject);
                        //nB.sendBroadCastSMS("SMS", e_mobile, smsBody);



                        SNMObj.OwnerNotification(e_FlatNo, APART_CODE_LOCAL_VAR, smsBody, emailSubject);

                    }
                    processVM.PaymentStatus = "Waitting For Collection";
                    processVM.Claim = 4;
                }
                else if (User.IsInRole("Treasurer"))
                {
                    processVM.Claim = 3;
                }
               // processVM.Claim = 3; //Approved from treasurer and received from Admin
                if (processVM.Id == 0)
                    _context.Add(processVM);
                else
                    _context.Update(processVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(processVM);
        }

        // GET: Flat
        public async Task<IActionResult> GetSplitListIndex()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var countTotalUser = _context.Users.Count(p => p.Flat_No != null && p.ApartCodeName== APART_CODE_LOCAL_VAR);
            if (countTotalUser > 0)
            {
                ViewBag.countTotalUserView = countTotalUser;
            }

            if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
            {
                return View(await _context.Processes.Where(e => e.Claim == 3 && e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
            }
            else
            {
                return View(await _context.Processes.Where(e=> e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
            }
        }


        [HttpGet]
        [Authorize]
        // GET: Flat/Create
        public IActionResult PaymentGateway(int id = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var usersList = (from users in _context.Users.Where(p => p.Flat_No != null && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                             select new SelectListItem()
                             {
                                 Value = users.Flat_No.ToString(),
                                 Text = users.Flat_No
                             }).Distinct().ToList();

            ViewBag.FlatNo = usersList;


            if (id == 0)
            {
                ViewData["readonly"] = "";
                return View(new ProcessVM());
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Processes.Find(id));
            }
        }


        [HttpGet]
        // GET: Flat/Create
        public IActionResult ViewDetails(int id = 0)
        {
            if (id == 0)
            {
                ViewData["readonly"] = "";
                return View(new ProcessVM());
            }
            else
            {
                ViewData["Readonly"] = "Readonly";
                return View(_context.Processes.Find(id));
            }
        }



        public string GetFlow(string flowName, Double amount)
        {
            string strResult="";
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            try
            {
                 //&& p.AmountLimit_MAX <= amount && p.Flow == flowName
                var GetRole = _context.ApprovalLimits
                                                .Where(p => p.AmountLimit_MIN <= amount&& p.AmountLimit_MAX >= amount && p.Flow == flowName && p.ApartCodeName== APART_CODE_LOCAL_VAR)                                                
                                                .Select(p => p.RoleName).First();
                strResult = GetRole.ToString();
            }
            catch(Exception ex)
            {
                strResult = "";
            }
            return strResult;
        }




        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var values = await _context.Processes.FindAsync(id);
            values.DeleteFlag = true;

            //_context.Processes.Remove(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Reject(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vals = await _context.Processes.FindAsync(id);
            vals.PaymentStatus = "Rejected";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public void SendNotification(string roleName, string textBody, string emailSubject)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var users = (from user in _context.Users
                              join userRole in _context.UserRoles
                              on user.Id equals userRole.UserId
                              join role in _context.Roles
                              on userRole.RoleId equals role.Id
                              where role.Name == roleName
                              && user.ApartCodeName == APART_CODE_LOCAL_VAR
                         select user).ToList();
            BroadCast nB = new BroadCast();
            foreach (var item in users)
            {
                string e_mobile = item.Mobile;
                string e_mail = item.Email;

                nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject);
                nB.sendBroadCastSMS("SMS", e_mobile, textBody);
            }

        }

        public void OwnerNotification(string flatNo, string textBody, string emailSubject)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];

            var flatVar = _context.Users
                                               .Where(p => p.Flat_No == flatNo && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                                               .Select(p => new { p.Mobile, p.Email, p.Flat_No }).ToList();
            BroadCast nB = new BroadCast();
            foreach (var item in flatVar)
            {
                var e_mail = item.Email;
                var e_mobile = item.Mobile;

                nB.sendBroadCastMail("Email", e_mail, textBody, emailSubject);
                nB.sendBroadCastSMS("SMS", e_mobile, textBody);
            }

        }




        public async Task<IActionResult> SMSUsers(string roleName)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var users = await (from user in _context.Users
                               join userRole in _context.UserRoles
                               on user.Id equals userRole.UserId
                               join role in _context.Roles
                               on userRole.RoleId equals role.Id
                               where role.Name == roleName
                               && user.ApartCodeName == APART_CODE_LOCAL_VAR
                               select user)
                                 .ToListAsync();

            return null;
        }

        //public string[] GetRolesForUser(string userName)
        //{

            // User user = db.api_login.FirstOrDefault(u => u.username.Equals(userName, StringComparison.CurrentCultureIgnoreCase)); //|| u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));
            //sending array
            //var roles = from r in db.api_login
            //            where r.role == r.ID &&
            //            r.username == userName
            //            select r.role;
            //if (roles != null)
            //    return roles.ToArray();
            //else
        //      return new string[] { }; 
        //}
        //public static List<OwnerVM> GetUsersInRole(string roleName)
        //{
            

            //List<OwnerVM> users = new List<OwnerVM>();

            //RoleManager roleManager = RoleManager.GetManager(SecurityConstants.ApplicationRolesProviderName);

            //if (roleManager.RoleExists(roleName))
            //{
            //    users = roleManager.GetUsersInRole(roleName).ToList();
            //}

        //    return users;
        //}

//        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString)) {
//    string queryString = "SELECT AspNetUsers.UserName FROM dbo.AspNetUsers INNER JOIN dbo.AspNetUserRoles ON " + "AspNetUsers.Id=AspNetUserRoles.UserId WHERE AspNetUserRoles.RoleID='" + id + "'";
//    SqlCommand command = new SqlCommand(queryString, connection);
//    command.Connection.Open();

//    List<string> @out = null;
//    dynamic reader = command.ExecuteReader();
//    while (reader.Read()) {
//        if (@out == null) @out = new List<string>();
//        @out.Add(reader.GetString(0));
//    }

//return @out;
//}


    }
}
