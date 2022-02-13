using FlatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly FlatDBContext _context;

        public CommitteeController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            return View(await _context.Committees.ToListAsync());
        }



        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new CommitteeVM());
            else
                return View(_context.Committees.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,MemberId,Roll,Remarks")] CommitteeVM committeeVM)
        {
            if (ModelState.IsValid)
            {
                if (committeeVM.Id == 0)
                    _context.Add(committeeVM);
                else
                    _context.Update(committeeVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(committeeVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var values = await _context.Committees.FindAsync(id);
            _context.Committees.Remove(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
