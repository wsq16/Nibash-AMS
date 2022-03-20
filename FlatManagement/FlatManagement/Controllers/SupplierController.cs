using FlatManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class SupplierController : Controller
    {
        private readonly FlatDBContext _context;
        
        public SupplierController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Suppliers.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }

        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new SupplierVM());
            else
                return View(_context.Suppliers.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Type,Amount,PrepairedBy,Start_Date,End_Date,Per_Address,Pre_Address,Email,Mobile,NID,ETIN")] SupplierVM supplierVM)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            ModelState.Clear();
            supplierVM.ApartCodeName = APART_CODE_LOCAL_VAR;
            if (ModelState.IsValid)                          
            {                                                
                if (supplierVM.Id == 0)                      
                    _context.Add(supplierVM);                
                else                                         	
                    _context.Update(supplierVM);             
                                                             
                await _context.SaveChangesAsync();           
                return RedirectToAction(nameof(Index));      
            }
            return View(supplierVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var values = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
