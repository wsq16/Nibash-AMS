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
    public class AdvancedPaymentController : Controller
    {
        private readonly FlatDBContext _context;

        public AdvancedPaymentController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Advanceds.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }

        // GET: Flat/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            string userName = User.Identity.Name;
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            //var usersList = (from users in _context.Users.Where(p => p.Flat_No != null)
            //                 select new SelectListItem()
            //                 {
            //                     Value = users.Flat_No.ToString(),
            //                     Text = users.Flat_No
            //                 }).Distinct().ToList();

            //ViewBag.FlatNo = usersList;
            /**
            var getFlatLst = (from u in _context.Users
                              join f in _context.FlatConfigs
                                     on u.Flat_No equals f.FlatNo
                              select new SelectListItem()
                              {
                                  Value = f.FlatNo.ToString(),
                                  Text = f.FlatNo
                              }).Distinct().ToList();
            getFlatLst.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.FlatNo = getFlatLst;
            
            var GetEmployeeList = _context.Employees
                                          .Where(p => p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                          .Select (p => p.FirstName+" "+p.LastName+" ("+p.Email+")").ToList();
            ViewBag.GetEmployeeList = GetEmployeeList;
            */

            //ViewBag.GetEmployeeList = new SelectList(_context.Employees.Where(p => p.ApartCodeName == APART_CODE_LOCAL_VAR).ToList(), "Id", "Email");

            /**
            var GetSuplierList = _context.Suppliers
                                         .Where(p => p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                         .Select(p => p.Email).ToList();
            ViewBag.GetSuplierList = GetSuplierList;
            */

            // ViewBag.GetSuplierList = new SelectList(_context.Suppliers.Where(p => p.ApartCodeName == APART_CODE_LOCAL_VAR).ToList(), "Id", "Email");

            var GetEmployeeList = (from u in _context.Employees
                                  .Where(p => p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                  select new SelectListItem()
                                  {
                                      Value = u.Email.ToString(),
                                      Text = u.Email+" ("+u.FirstName+" "+u.LastName+")"
                                  }).Distinct().ToList();

            ViewBag.GetEmployeeList = GetEmployeeList;



            var GetSuplierList = (from u in _context.Suppliers
                                  .Where(p => p.ApartCodeName == APART_CODE_LOCAL_VAR)
                                  select new SelectListItem()
                              {
                                  Value = u.Email.ToString(),
                                  Text = u.Email
                              }).Distinct().ToList();

            ViewBag.GetSuplierList = GetSuplierList;


            ViewBag.Month = new SelectList(Enum.GetValues(typeof(Month)), DateTime.Now.ToString("MMMM"));

            ViewBag.Years = new SelectList(Enumerable.Range(DateTime.Today.Year, 20).Select(x =>
              new SelectListItem()
              {
                  Text = x.ToString(),
                  Value = x.ToString()
              }), "Value", "Text");

            
            if (id == 0)
                return View(new AdvancedVM());
            else
                return View(_context.Advanceds.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(AdvancedVM advancedVMObj)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var APART_APART_LOCAL_VAR = HttpContext.Request.Cookies["COMNAME"];
            advancedVMObj.CollectionBy = User.Identity.Name;
            advancedVMObj.ApartCodeName = APART_CODE_LOCAL_VAR;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (advancedVMObj.Id == 0)
                    _context.Add(advancedVMObj);
                else
                    _context.Update(advancedVMObj);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advancedVMObj);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var valObj = await _context.Advanceds.FindAsync(id);
            _context.Advanceds.Remove(valObj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
