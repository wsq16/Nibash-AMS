using FlatManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ContractController : Controller
    {
        private readonly FlatDBContext _context;
        const string APARTCODEVAR = "_ApartCodeSession";
        
        public ContractController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Contracts.Where(e=> e.ApartCodeName== APART_CODE_LOCAL_VAR).ToListAsync());
        }



        // GET: Flat/Create
        [Authorize]
        public IActionResult AddOrEdit(int id = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var billType = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_TYPE") && b.ApartCodeName== APART_CODE_LOCAL_VAR)
                            select new SelectListItem()
                            {
                                Value = b.EnumText.ToString(),
                                Text = b.EnumText
                            }).ToList();

            ViewBag.billType = billType;


            var billFrequency = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_FREQUENCY") && b.ApartCodeName == APART_CODE_LOCAL_VAR)
                            select new SelectListItem()
                            {
                                Value = b.EnumText.ToString(),
                                Text = b.EnumText
                            }).ToList();

            ViewBag.billFrequency = billFrequency;
            

            if (id == 0)
                return View(new ContractVM());
            else
                return View(_context.Contracts.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,ContractName,ContactPerson,StartDate,EndDate,Description,BillType,BillFrequency")] ContractVM contractVM)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            ModelState.Clear();
            contractVM.ApartCodeName = APART_CODE_LOCAL_VAR;
            if (ModelState.IsValid)
            {
                if (contractVM.Id == 0)
                    _context.Add(contractVM);
                else
                    _context.Update(contractVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contractVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var values = await _context.Contracts.FindAsync(id);
            _context.Contracts.Remove(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
