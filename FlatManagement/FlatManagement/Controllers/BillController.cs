using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlatManagement.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols;
using FlatManagement.Utility;
using Microsoft.AspNetCore.Authorization;

namespace FlatManagement.Controllers
{
    public class BillController : Controller
    {
        public IConfiguration _configuration;
        private readonly FlatDBContext _context;

        public BillController(IConfiguration configuration, FlatDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: Bill
        [Authorize]
        public async Task<IActionResult> Index(string billStatus="", int trn=0, string billId="")
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            //var item = _context.Bills.Select(x => new SelectListItem()
            //{
            //    Text = x.Amount.ToString(),
            //    Value = x.Id.ToString()
            //}).ToList();
            //var vm = new BillVM()
            //{
            //    BillItems = item
            //};

            var userName = User.FindFirstValue(ClaimTypes.Name);

            

            //var user2 = _context.Users.SingleOrDefault(u => u.UserName == userName);
            if (trn == 1)
            {

                //string vId = "";
                if (billId.Length > 0)
                {
                    //string bIIdStr = billId.Remove(billId.Length - 1, 1);
                    string vId = "(" + billId + ")";
                
                string connectionString = _configuration.GetConnectionString("DBConnectionString");
                SqlConnection sqlConn = new SqlConnection(connectionString);
                string query = "UPDATE Bills SET BillStatus = 'Paid' WHERE Id IN " + vId;
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                sqlConn.Open();
                int i = cmd.ExecuteNonQuery();
                sqlConn.Close();
                
                }
            }



            var userFlat = (from u in _context.Users
                            where u.UserName == userName
                            select u.Flat_No).FirstOrDefault();
            ViewBag.OwnFlatNo = userFlat;

            if (User.IsInRole("Admin") || User.IsInRole("Supervisor"))
            {
                if (billStatus == "")
                {
                    return View(await _context.Bills.Where(e => e.BillStatus == "Initial" && e.ApartCodeName== APART_CODE_LOCAL_VAR).OrderByDescending(e => e.Id).ToListAsync());
                }else if(billStatus == "All")
                {
                    return View(await _context.Bills.Where(e =>e.ApartCodeName == APART_CODE_LOCAL_VAR).OrderByDescending(e => e.Id).ToListAsync());
                }
                else
                {
                    return View(await _context.Bills.Where(e => e.BillStatus == billStatus && e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
                }
            }
            else if (User.IsInRole("FlatOwner"))
            {
                if (billStatus == "All"|| billStatus == "")
                {
                    return View(await _context.Bills.Where(e => e.FlatNo == userFlat||e.BillFor == userFlat && e.ApartCodeName == APART_CODE_LOCAL_VAR).OrderByDescending(e => e.BillStatus).ThenBy(e=>e.Id).ToListAsync());
                }
                else
                {
                    return View(await _context.Bills.Where(e => e.FlatNo == userFlat && e.BillStatus == billStatus && e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
                }
            }
                
            else
                return View(await _context.Bills.Where(e => e.BillStatus == billStatus && e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());

            //
        }

        // GET: Bill/Create
        [Authorize]
        public IActionResult AddOrEdit(int id=0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            //var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
            //                 select new SelectListItem()
            //                 {
            //                     Value = users.Flat_No.ToString(),
            //                     Text = users.Flat_No
            //                 }).Distinct().ToList();

            //ViewBag.FlatNo = usersList;

            var getFlatLst = (from u in _context.Users
                              join f in _context.FlatConfigs
                                     on u.Flat_No equals f.FlatNo
                              where u.ApartCodeName == APART_CODE_LOCAL_VAR
                              select new SelectListItem()
                              {
                                  Value = f.FlatNo.ToString(),
                                  Text = f.FlatNo+" ("+u.FirstName+" "+u.LastName+")"
                              }).Distinct().ToList();

            ViewBag.FlatNo = getFlatLst;

            var billType = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_TYPE") && b.ApartCodeName== APART_CODE_LOCAL_VAR)
                           select new SelectListItem()
                           {
                               Value = b.EnumText.ToString(),
                               Text = b.EnumText
                           }).ToList();

            ViewBag.billType = billType;

            #region ViewBag
            List<SelectListItem> billFor = new List<SelectListItem>() {
        new SelectListItem {
            Text = "Owners", Value = "1"
        },
        new SelectListItem {
            Text = "Common", Value = "2"
        },
        new SelectListItem {
            Text = "Others", Value = "3"
        },
    };
            ViewBag.billFor = billFor;
            #endregion


            //var billFor = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_FOR") )
            //               select new SelectListItem()
            //                 {
            //                     Value = b.EnumText.ToString(),
            //                     Text = b.EnumText
            //               }).ToList();

            


            if (id == 0)
                return View(new BillVM());
            else
                return View(_context.Bills.Find(id));
        }





        // POST: Bill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddOrEdit([Bind("Id,BillType,Amount,BillDate,BillFor,FlatNo,DueDate,Remarks,PreparedBy,EntryDate")] BillVM bill)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            string chkBillFor = bill.BillFor.ToString();
            if (chkBillFor == "1")
            {
                bill.BillFor = "Owners";

                var OwnerMobile = _context.Users
                        .Where(p => p.Flat_No == bill.FlatNo && p.ApartCodeName== APART_CODE_LOCAL_VAR)
                        .Select(p => p.Mobile).First();

                var OwnerEmail = _context.Users
                        .Where(p => p.Flat_No == bill.FlatNo && p.ApartCodeName == APART_CODE_LOCAL_VAR)
                        .Select(p => p.Email).First();


                string emailSubject = "A Bill Created";
                string smsBody = "A Bill Initiated. Please login to system and pay the bill on time.";
                BroadCast nB = new BroadCast();
             
                nB.sendBroadCastSMS("SMS", OwnerMobile, smsBody);
                nB.sendBroadCastMail("Email", OwnerEmail, smsBody, emailSubject);
            }
            else if (chkBillFor == "2")
            {
                bill.BillFor = "Common";
            }
            else if (chkBillFor == "3")
            {
                bill.BillFor = "Others";
            }
            bill.BillStatus = "Initial";
            bill.ApartCodeName = APART_CODE_LOCAL_VAR;
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if(bill.Id == 0) 
                _context.Add(bill);
                else
                    _context.Update(bill);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }


        public IActionResult ViewBillData(int id = 0)
        {
            if (id > 0)
                return View(_context.Bills.Find(id));
            else
                return View(new BillVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ViewBillData(BillVM bill)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];

            var GetMobileNumber = _context.Users
                                               .Where(p => p.Flat_No == bill.FlatNo)
                                               .Select(p => p.Mobile).FirstOrDefault();

                string emailSubject = "Cash Received";
                string smsBody = "A Bill Initiated. Please login to system and pay the bill on time.";
                BroadCast nB = new BroadCast();

             nB.sendBroadCastSMS("SMS", GetMobileNumber, smsBody);
            // nB.sendBroadCastMail("Email", OwnerEmail, smsBody, emailSubject);

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                //if (bill.Id == 0)
                //    _context.Add(bill);
                //else

                var values = await _context.Bills.FindAsync(bill.Id);
                values.ReceivedAmount = bill.ReceivedAmount;
                values.ReceivedAmountNotes = bill.ReceivedAmountNotes;
                values.BillStatus = "Paid";
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(bill);
        }


        public string GetInfo(string mNum)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            string constr = _configuration.GetConnectionString("DBConnectionString");
            string mobileNumStr = "";

            List<SelectListItem> items = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT Email, Mobile FROM AspNetUsers WHERE AspNetUsers.UserName='" + mNum + "' AND ApartCodeName='" + APART_CODE_LOCAL_VAR + "' AND IsActive='true'";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            mobileNumStr = sdr["Mobile"].ToString();
                        }
                    }
                    con.Close();
                }
            }
            return mobileNumStr;
        }

        // GET: Bill/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bill = await _context.Bills.FindAsync(id);
            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
