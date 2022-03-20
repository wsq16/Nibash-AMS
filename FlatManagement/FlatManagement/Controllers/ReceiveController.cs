using FlatManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ReceiveController : Controller
    {
        private readonly FlatDBContext _context;

        public ReceiveController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Receives.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }

        // GET: Flat/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            string userName = User.Identity.Name;
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];

            var getFlatLst = (from u in _context.Users
                              join f in _context.FlatConfigs
                                     on u.Flat_No equals f.FlatNo
                              where u.ApartCodeName == APART_CODE_LOCAL_VAR
                              select new SelectListItem()
                              {
                                  Value = f.FlatNo.ToString(),
                                  Text = f.FlatNo + " (" + u.FirstName + " " + u.LastName + ")"
                              }).Distinct().ToList();

            ViewBag.FlatNo = getFlatLst;


            var pendingBillList = (from c in _context.Bills.Where(p => p.BillStatus.Contains("Initial") && p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                 select new SelectListItem()
                                 {
                                     Value = c.Id.ToString(),
                                     Text = c.FlatNo+"-"+c.Amount
                                 }).ToList();

            ViewBag.pendingBillList = pendingBillList;




            ViewBag.Month = new SelectList(Enum.GetValues(typeof(Month)), DateTime.Now.ToString("MMMM"));

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 20).Select(x =>
              new SelectListItem()
              {
                  Text = x.ToString(),
                  Value = x.ToString()
              }), "Value", "Text");


            if (id == 0)
                return View(new ReceiveVM());
            else
                return View(_context.Receives.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(ReceiveVM receiveVMObj)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];
            receiveVMObj.CollectionBy = User.Identity.Name;
            receiveVMObj.ApartCodeName = APART_CODE_LOCAL_VAR;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (receiveVMObj.Id == 0)
                    _context.Add(receiveVMObj);
                else
                    _context.Update(receiveVMObj);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receiveVMObj);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valObj = await _context.Receives.FindAsync(id);
            _context.Receives.Remove(valObj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    
    }   
}
