using FlatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class OwnerController : Controller
    {
        private readonly FlatDBContext _context;

        public OwnerController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            return View(await _context.Owners.ToListAsync());
        }



        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new OwnerVM());
            else
                return View(_context.Owners.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,FirstName,LastName,Email,Mobile,DOB,NID,ETIN,PassportNo,Per_Address,pre_Address")] OwnerVM ownerVM)
        {
            if (ModelState.IsValid)
            {
                if (ownerVM.Id == 0)
                    _context.Add(ownerVM);
                else
                    _context.Update(ownerVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ownerVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var owners = await _context.Owners.FindAsync(id);
            _context.Owners.Remove(owners);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
