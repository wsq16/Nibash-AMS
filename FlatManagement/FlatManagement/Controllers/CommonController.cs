using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult FileDelete(string filepath, string table, string field, string id)
        {
            string tableName = "";
            tableName = table;
           // bool returnval = new CommonRepo().FileDelete(tableName, field, id);
            bool returnval = true;
            if (returnval)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "" + filepath;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            return Json(returnval, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
