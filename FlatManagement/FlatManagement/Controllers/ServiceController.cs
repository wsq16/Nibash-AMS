using FlatManagement.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ServiceController : Controller
    {
        private readonly FlatDBContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        

        public ServiceController(FlatDBContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Flat
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.Services.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());
        }



        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var billType = (from b in _context.EnumValues.Where(b => b.Value.Contains("BILL_TYPE") && b.ApartCodeName == APART_CODE_LOCAL_VAR)
                            select new SelectListItem()
                            {
                                Value = b.EnumText.ToString(),
                                Text = b.EnumText
                            }).ToList();

            ViewBag.billType = billType;


            if (id == 0)
                return View(new ServiceVM());
            else
                return View(_context.Services.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(ServiceVM serviceVM, List<IFormFile> files)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            try
            {
                if (files != null)
                {
                    int countFile = 0;
                    foreach (var file in files)
                    {
                        countFile++;
                        if (file.Length > 0)
                        {
                            string uniqueFileName = null;
                            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "DocFiles");
                            uniqueFileName = Path.GetFileName(file.FileName); /*DateTime.Now.ToString("yymmssfff") + "_" +*/
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                        if (countFile == 1)
                                        {
                                           serviceVM.FileUpload1 = uniqueFileName;
                                        }
                                else if (countFile == 2)
                                        {
                                            serviceVM.FileUpload2 = uniqueFileName;
                                        }
                                else if (countFile == 3)
                                        {
                                            serviceVM.FileUpload3 = uniqueFileName;
                                        }

                                    await file.CopyToAsync(fileStream);
                                }


                        }
                    }
                }



                //string uniqueFileName = null;
                //string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Files");
                //uniqueFileName = DateTime.Now.ToString("yymmssfff") + "_" + Path.GetFileName(file.FileName);
                 // uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(uploadFile.FileName);
                //string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //using (var fileStream = new FileStream(filePath, FileMode.Create))
                //{
                //    await file.CopyToAsync(fileStream);
                //}
                //serviceVM.FileUpload = uniqueFileName;

                            
            }catch(Exception ex)
            {
                Console.WriteLine("Error in file upload:" + ex.ToString());
            }

            serviceVM.ApartCodeName = APART_CODE_LOCAL_VAR;

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (serviceVM.Id == 0)
                    _context.Add(serviceVM);
                else
                    _context.Update(serviceVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(serviceVM);
        }

       
        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var values = await _context.Services.FindAsync(id);
            _context.Services.Remove(values);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public JsonResult FileDelete(string filepath, string table, string field, string id)
        {
            int tid = Convert.ToInt32(id);
            string tableName = "";
            tableName = table;
            bool returnval = true;
            if (returnval)
            {
                try
                {
                    string[] filepaths = filepath.Split('-');
                    foreach (string fPath in filepaths)
                    {
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DocFiles", fPath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }

                    var serviceTbl = _context.Services.FirstOrDefault(u => u.Id == tid);
                    serviceTbl.FileUpload1 = "";
                    serviceTbl.FileUpload2 = "";
                    serviceTbl.FileUpload3 = "";
                    _context.SaveChanges();
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }
            return Json(returnval, new Newtonsoft.Json.JsonSerializerSettings());
        }




    }
}
