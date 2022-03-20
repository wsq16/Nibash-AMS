using FlatManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class ApprovalLimitController : Controller
    {
        private readonly FlatDBContext _context;
        public IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        

        public ApprovalLimitController(IConfiguration configuration, FlatDBContext context, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

            // GET: Flat
        public async Task<IActionResult> Index()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            return View(await _context.ApprovalLimits.Where(e => e.ApartCodeName == APART_CODE_LOCAL_VAR).ToListAsync());

        }



        // GET: Flat/Create
        public IActionResult AddOrEdit(int id = 0)
        {
            var usersRole = (from rolesStr in _context.Roles
                            select new SelectListItem()
                            {
                                Value = rolesStr.Name.ToString(),
                                Text = rolesStr.Name
                            }).Distinct().ToList();

            ViewBag.UserRoleList = usersRole;

            if (id == 0)
                return View(new ApprovalLimitVM());
            else
                return View(_context.ApprovalLimits.Find(id));
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(ApprovalLimitVM ApprovalLimitVM)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            ApprovalLimitVM.Flow = ApprovalLimitVM.FlowTypes.ToString();

            ModelState.Clear();
            ApprovalLimitVM.ApartCodeName = APART_CODE_LOCAL_VAR;

            if (ModelState.IsValid)
            {
                if (ApprovalLimitVM.Id == 0)
                    _context.Add(ApprovalLimitVM);
                else
                    _context.Update(ApprovalLimitVM);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ApprovalLimitVM);
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
