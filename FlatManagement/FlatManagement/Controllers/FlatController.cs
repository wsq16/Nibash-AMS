using FlatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class FlatController : Controller
    {
        private readonly FlatDBContext _context;

        public FlatController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: Flat
        //public async Task<IActionResult> Index()
       // {
           // return View(await _context.Flats.ToListAsync());
       // }



        // GET: Flat/Create
        //public IActionResult AddOrEdit(int id = 0)
       // {
           // if (id == 0)
           //     return View(new FlatVM());
          //  else
               // return View(_context.Flats.Find(id));
        //}

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,FlatNo,Remarks")] FlatVM flatVM)
        {
            if (ModelState.IsValid)
            {
                if (flatVM.Id == 0)
                    _context.Add(flatVM);
                else
                    _context.Update(flatVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flatVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


           // var valObj = await _context.Flats.FindAsync(id);
          //  _context.Flats.Remove(valObj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
