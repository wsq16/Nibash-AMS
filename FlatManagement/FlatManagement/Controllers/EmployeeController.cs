using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlatManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace FlatManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly FlatDBContext _context;
       
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmployeeController(FlatDBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
       

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employees.ToListAsync());
        }


        public IActionResult ViewEmpData(int id = 0)
        {
            if(id>0)
                return View(_context.Employees.Find(id));
            else
                return View(new EmployeeVM());
        }





        // GET: Employee/Create
        public IActionResult AddOrEdit(int id = 0)
        {

            var empTYpe = (from b in _context.EnumValues.Where(b => b.Value.Contains("EMPLOYEE_TYPE"))
                           select new SelectListItem()
                           {
                               Value = b.EnumText.ToString(),
                               Text = b.EnumText
                           }).ToList();

            ViewBag.empTYpe = empTYpe;



            if (id == 0)
                return View(new EmployeeVM());
            else
                return View(_context.Employees.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(EmployeeVM employeeVM, IFormFile uploadFile)
        {

            //employeeVM.PicLoc = "no_image.png";

            //employeeVM.PicLoc = "no_image.png";
            //if (uploadFile != null && uploadFile.Length > 0)
            //{
            //    var fileName = Path.GetFileName(uploadFile.FileName);
            //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            //    employeeVM.PicLoc = fileName;

            //    using (var fileSrteam = new FileStream(filePath, FileMode.Create))
            //    {
            //        await uploadFile.CopyToAsync(fileSrteam);
            //    }
            //}
            //else
            //{
            //    employeeVM.PicLoc = "no_image.png";
            //}

            //Process2
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
                employeeVM.PicLoc = uniqueFileName;
            }

            employeeVM.EntryBy = User.Identity.IsAuthenticated ? HttpContext.User.Identity.Name : "-";
            employeeVM.EntryDate = DateTime.Now;
            //else
            //{
            //    employeeVM.PicLoc = "no_image.png";
            //}


            //if (employeeVM.PicLoc.Length > 0)
            //{
            //    //Save image to wwwroot/image
            //    string wwwRootPath = _hostingEnvironment.WebRootPath;
            //    string fileName = Path.GetFileNameWithoutExtension(employeeVM.ImageFile.FileName);
            //    string extension = Path.GetExtension(employeeVM.ImageFile.FileName);
            //    employeeVM.PicLoc = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //    string path = Path.Combine(wwwRootPath + "/images/", fileName);
            //    using (var fileStream = new FileStream(path, FileMode.Create))
            //    {
            //        await employeeVM.ImageFile.CopyToAsync(fileStream);
            //    }
            //}

                ModelState.Clear();
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (employeeVM.Id == 0)
                            _context.Add(employeeVM);
                        else
                            _context.Update(employeeVM);

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {

                    }
                }

            return View(employeeVM);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       

    }
}
