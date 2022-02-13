using FlatManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class DemoChkBoxController : Controller
    {
        public IConfiguration _configuration;
        private readonly FlatDBContext _context;

        public DemoChkBoxController(IConfiguration configuration, FlatDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<DemoChkBoxClass> categoryList = new List<DemoChkBoxClass>();
            categoryList = (from category in _context.Category
                            select category).ToList();
            return View(categoryList);
        }


        [HttpPost]
        public IActionResult Index(List<DemoChkBoxClass> objCategory)
        {
            var countChecked = 0;
            var countUnChecked = 0;
            for(int i=0; i<objCategory.Count(); i++)
            {
                if(objCategory[i].checkboxAnswer == true)
                {
                    countChecked = countChecked + 1;
                }
                else
                {
                    countUnChecked = countUnChecked + 1;
                }
            }
            return View(objCategory);

        }
    }
}
