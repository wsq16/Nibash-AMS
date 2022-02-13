using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlatManagement.Models;

namespace FlatManagement.Controllers
{
    public class EnumValuesController : Controller
    {
        private readonly FlatDBContext _context;

        public EnumValuesController(FlatDBContext context)
        {
            _context = context;
        }

        // GET: EnumValues
        public async Task<IActionResult> Index()
        {
            return View(await _context.EnumValues.ToListAsync());
        }

        // GET: EnumValues/Create
        public IActionResult AddOrEdit(int id=0)
        {
            ViewBag.ENUMTYPES = new SelectList(Enum.GetValues(typeof(EnumValueType)));
            if (id == 0)
                return View(new EnumModel());
            else
                return View(_context.EnumValues.Find(id));
        }

        // POST: EnumValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Value,EnumText")] EnumModel enumModel)
        {
            if (ModelState.IsValid)
            {
                if (enumModel.Id == 0)
                    _context.Add(enumModel);
                else
                    _context.Update(enumModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(enumModel);
        }

        // GET: Bill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var EnumModel = await _context.EnumValues.FindAsync(id);
            _context.EnumValues.Remove(EnumModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
