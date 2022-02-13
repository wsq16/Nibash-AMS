using FlatManagement.Models;
using FlatManagement.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class CommonFundController : Controller
    {
        private readonly FlatDBContext _context;

        public CommonFundController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            return View(await _context.CommonFunds.ToListAsync());
        }

        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
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
                             select new SelectListItem()
                             {
                                 Value = f.FlatNo.ToString(),
                                 Text = f.FlatNo
                             }).Distinct().ToList();

            ViewBag.FlatNo = getFlatLst;

            //where u.LastName.Contains("fra") && selectedRoles.Contains(c.CompanyRoleId)

            ViewBag.Month = new SelectList(Enum.GetValues(typeof(Month)), DateTime.Now.ToString("MMMM"));

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 20).Select(x =>
              new SelectListItem()
              {
                  Text = x.ToString(),
                  Value = x.ToString()
              }), "Value", "Text");


            //List<EnumModel>fundType = new List<EnumModel>();
            //fundType = (from c in _context.EnumValues select c).Where(u => u.Value == "FUND_TYPE").ToList();
            //fundType.Insert(0, new EnumModel { Id = 0, Value = "--Select Fund Type--" });
            //ViewBag.fundType = fundType;

            var fundType = (from b in _context.EnumValues.Where(b => b.Value.Contains("FUND_TYPE"))
                           select new SelectListItem()
                           {
                               Value = b.EnumText.ToString(),
                               Text = b.EnumText
                           }).ToList();

            ViewBag.fundType = fundType;



            if (id == 0)
                return View(new CommonFundVM());
            else
                return View(_context.CommonFunds.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(CommonFundVM CommonFundVM)
        {
            string flat_No = CommonFundVM.FlatNo;

            var GetOwner = _context.Users
                                               .Where(p => p.Flat_No == flat_No)
                                               .Select(p => p.FlatOwner).First();

            CommonFundVM.FlatOwner = GetOwner;

            CommonFundVM.CollectionBy = User.Identity.Name;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (CommonFundVM.Id == 0)
                    _context.Add(CommonFundVM);
                else
                    _context.Update(CommonFundVM);


                string textBody = "Fund Collection from the flat owner of " + CommonFundVM.FlatNo + " for the month:" + CommonFundVM.Month + " Amount: " + CommonFundVM.Amount + " Collection Date: " + CommonFundVM.CollectionDate;
                string textSubject = "Fund Collection";

                SendNotificationMaster SNMObj = new SendNotificationMaster(_context);
                SNMObj.SendNotification("All","Committee", textBody, textSubject, null);
                SNMObj.SendNotification("All","Treasurer", textBody, textSubject, null);
                SNMObj.OwnerNotification(CommonFundVM.FlatNo, textBody, textSubject);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(CommonFundVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valObj = await _context.CommonFunds.FindAsync(id);
            _context.CommonFunds.Remove(valObj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
