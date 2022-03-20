using FlatManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class CompanyController : Controller
    {

        private readonly FlatDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public CompanyController(FlatDBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyInfo.ToListAsync());
        }


        // GET: EnumValues/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new CompanyVM());
            else
                return View(_context.CompanyInfo.Find(id));
        }

        // POST: EnumValues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[Bind("Id,CompanyName,CodeName,LogoUri,Mobile,Email,Address,License,IsActive")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(CompanyVM CompanyObj, IFormFile uploadFile)
        {
            try
            {
                string uniqueFileName = null;
                if (uploadFile != null && uploadFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = DateTime.Now.ToString("yymmssfff") + "_" + Path.GetFileName(uploadFile.FileName);
                    // uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadFile.CopyToAsync(fileStream);
                    }
                    CompanyObj.LogoUri = uniqueFileName;
                }
                else
                {
                    CompanyObj.LogoUri = "no_image_apartment.png";
                }
            }catch(Exception ex)
            {
               Console.WriteLine(ex.ToString());
            }

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (CompanyObj.Id == 0)
                    _context.Add(CompanyObj);
                else
                    _context.Update(CompanyObj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(CompanyObj);
        }

        [AcceptVerbs("Get", "Post")]//[HttpGet][HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsCodeNameInUse(string CodeName)
        {
            var codeNameStr = await _context.CompanyInfo.Where(e => e.ApartCodeName == CodeName).ToListAsync();

            if (codeNameStr.Count == 0)
            {
                return Json(true);
            }
            else
            {
                return Json($"Code Name {CodeName} is already in use.");
            }
        }


    }
}
