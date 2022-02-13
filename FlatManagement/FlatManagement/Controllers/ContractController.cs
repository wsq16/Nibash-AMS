using FlatManagement.Models;
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

        public ContractController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contracts.ToListAsync());
        }



        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            var billType = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_TYPE"))
                            select new SelectListItem()
                            {
                                Value = b.EnumText.ToString(),
                                Text = b.EnumText
                            }).ToList();

            ViewBag.billType = billType;


            var billFrequency = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_FREQUENCY"))
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
