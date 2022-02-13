using FlatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class TenantController : Controller
    {
        private readonly FlatDBContext _context;

        public TenantController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tenants.ToListAsync());
        }



        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new TenantVM());
            else
                return View(_context.Tenants.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,ownerId,FirstName,LastName,Email,Mobile,DOB,NID,ETIN,PassportNo,Per_Address,pre_Address")] TenantVM tenantVM)
        {
            if (ModelState.IsValid)
            {
                if (tenantVM.Id == 0)
                    _context.Add(tenantVM);
                else
                    _context.Update(tenantVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tenantVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var values = await _context.Tenants.FindAsync(id);
            _context.Tenants.Remove(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
