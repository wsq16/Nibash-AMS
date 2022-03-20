using FlatManagement.Models;
using FlatManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatManagement.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private readonly FlatDBContext _context;

        const string APARTCODEVAR = "_ApartCodeSession";
        const string APARTCOMPANYNAME = "_ApartCompanyName";
        const string APARTCOMPANYLOGO = "_ApartCompanyLogo";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, 
                                SignInManager<ApplicationUser> signInManager, FlatDBContext context)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]//[HttpGet][HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, false);
                
                if (result.Succeeded)
                {

                   var getValidUser = (from u in _context.Users
                                    where u.IsActive == true && u.Email== model.Email
                                       select u.UserName).FirstOrDefault();
                    if (getValidUser != null)
                    {
                        try
                        {
                            string userName = getValidUser;
                            var getApartCodeName = (from u in _context.Users
                                                    where u.IsActive == true && u.UserName == userName
                                                    select u.ApartCodeName).FirstOrDefault();
                            var getCompanyName = (from u in _context.CompanyInfo
                                                  where u.IsActive == true && u.ApartCodeName == getApartCodeName
                                                  select u.CompanyName).FirstOrDefault();
                            var getCompanyLogo = (from u in _context.CompanyInfo
                                                  where u.IsActive == true && u.ApartCodeName == getApartCodeName
                                                  select u.LogoUri).FirstOrDefault();

                            HttpContext.Response.Cookies.Append("COMCODE", getApartCodeName);
                            HttpContext.Response.Cookies.Append("COMNAME", getCompanyName);
                            HttpContext.Response.Cookies.Append("COMLOGO", getCompanyLogo);


                            HttpContext.Session.SetString(APARTCODEVAR, getApartCodeName);
                            HttpContext.Session.SetString(APARTCOMPANYNAME, getCompanyName);
                            HttpContext.Session.SetString(APARTCOMPANYLOGO, getCompanyLogo);

                            ViewBag.CodeName = HttpContext.Session.GetString(APARTCODEVAR);
                            ViewBag.CompanyName = HttpContext.Session.GetString(APARTCOMPANYNAME);
                            ViewBag.CompanyLogo = HttpContext.Session.GetString(APARTCOMPANYLOGO);

                           // if (!HttpContext.Request.Cookies.ContainsKey("first_request"))
                           // {
                          //      HttpContext.Response.Cookies.Append("user_id", "1");
                           //     HttpContext.Response.Cookies.Append("first_request", DateTime.Now.ToString());
                               // return Content("Welcome, new visitor!");
                           // }
                           // else
                          //  {
                           //     DateTime firstRequest = DateTime.Parse(HttpContext.Request.Cookies["first_request"]);
                               // return Content("Welcome back, user! You first visited us on: " + firstRequest.ToString());
                           // }
                        }
                        catch
                        {
                            ViewBag.CodeName = "";
                            ViewBag.CompanyName = "";
                            ViewBag.CompanyLogo = "";
                        }

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("index", "home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    }



                    
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            Remove("COMCODE");
            Remove("COMNAME");
            Remove("COMLOGO");
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignOff()
        {
            await signInManager.SignOutAsync();
            //Remove("COMCODE");
            //Remove("COMNAME");
            //Remove("COMLOGO");
            return RedirectToAction("index", "home");
        }

        /// <summary>  
        /// Delete the key  
        /// </summary>  
        /// <param name="key">Key</param>  
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var FlatList = (from f in _context.FlatConfigs.Where(e => e.ApartCodeName== APART_CODE_LOCAL_VAR && !_context.Users.Select(m => m.Flat_No)
                                                 .Contains(e.FlatNo))
                            select new SelectListItem()
                            {
                                Value = f.FlatNo.ToString(),
                                Text = f.FlatNo
                            }).ToList();
            ViewBag.FlatList = FlatList;



            //Get Identity Users
            var usersList = (from users in _context.Users.Where(p => p.ApartCodeName== APART_CODE_LOCAL_VAR && p.IsActive==true && p.Tenant == false)
                             select new SelectListItem()
                             {
                                 Value = users.UserName.ToString(),
                                 Text = users.Flat_No+"- ("+users.UserName+")" 
                             }).ToList();

            ViewBag.FlatOwner = usersList;

            //var users = from u in _context.Users.Where(p => p.Tenant == false)
            //            select u;
            // ViewBag.FlatOwner = usersList.Select(x => new SelectListItem { Text = x.UserName, Value = x.Id }).ToList();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SuperUserRegister()
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            var FlatList = (from f in _context.FlatConfigs.Where(e => e.ApartCodeName== APART_CODE_LOCAL_VAR && !_context.Users.Select(m => m.Flat_No)
                                                 .Contains(e.FlatNo))
                            select new SelectListItem()
                            {
                                Value = f.FlatNo.ToString(),
                                Text = f.FlatNo
                            }).ToList();
            ViewBag.FlatList = FlatList;


            var ApartmentList = (from f in _context.CompanyInfo.Where(e => e.IsActive==true)
                            select new SelectListItem()
                            {
                                Value = f.ApartCodeName.ToString(),
                                Text = f.CompanyName.ToString()
                            }).ToList();
            ViewBag.ApartmentList = ApartmentList;


            //Get Identity Users
            var usersList = (from users in _context.Users.Where(p => p.ApartCodeName== APART_CODE_LOCAL_VAR && p.IsActive==true && p.Tenant == false && p.Flat_No!=null)
                             select new SelectListItem()
                             {
                                 Value = users.UserName.ToString(),
                                 Text = users.Flat_No + "- (" + users.UserName + ")"
                             }).ToList();

            ViewBag.FlatOwner = usersList;

            //var users = from u in _context.Users.Where(p => p.Tenant == false)
            //            select u;
            // ViewBag.FlatOwner = usersList.Select(x => new SelectListItem { Text = x.UserName, Value = x.Id }).ToList();
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var APART_CODE_LOCAL_VAR = HttpContext.Request.Cookies["COMCODE"];
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (model.TenantValue == "User")
                {
                    model.Tenant = false;
                    model.FlatOwner = model.Email;
                }
                else
                {
                    model.Tenant = true;
                }


                var user = new ApplicationUser 
                {
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    NID = model.NID,
                    ETIN = model.ETIN,
                    PassportNo = model.PassportNo,
                    Per_Address = model.Per_Address,
                    pre_Address = model.pre_Address,
                    Flat_No = model.Flat_No,
                    UserRole = model.UserRole,
                    Tenant = model.Tenant,
                    FlatOwner = model.FlatOwner,
                    ApartCodeName = APART_CODE_LOCAL_VAR,
                    IsActive = true
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
                return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SuperUserRegister(RegisterViewModel model)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (model.TenantValue == "User")
                {
                    model.Tenant = false;
                    model.FlatOwner = model.Email;
                }
                else if(model.TenantValue == "Tenant")
                {
                    model.Tenant = true;
                }
                else if (model.TenantValue == "Other")
                {
                    model.FlatOwner = model.Email;
                    model.Flat_No = "-";
                    model.Tenant = false;
                }


                    var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Mobile = model.Mobile,
                    NID = model.NID,
                    ETIN = model.ETIN,
                    PassportNo = model.PassportNo,
                    Per_Address = model.Per_Address,
                    pre_Address = model.pre_Address,
                    Flat_No = model.Flat_No,
                    UserRole = model.UserRole,
                    Tenant = model.Tenant,
                    FlatOwner = model.FlatOwner,
                    ApartCodeName = model.ApartmentList,
                    IsActive = true

                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        //public ActionResult UsersWithRoles()
        //{
        //var usersWithRoles = (from user in _context.Users
        //                      select new
        //                      {
        //                          UserId = user.Id,
        //                          Username = user.UserName,
        //                          Email = user.Email,
        //                          RoleNames = (from userRole in user.UserRole
        //                                       join role in _context.Roles on userRole..RoleId
        //                                       equals role.Id
        //                                       select role.Name).ToList()
        //                      }).ToList().Select(p => new Users_in_Role_ViewModel()

        //                      {
        //                          UserId = p.UserId,
        //                          Username = p.Username,
        //                          Email = p.Email,
        //                          Role = string.Join(",", p.RoleNames)
        //                      });
        //return View(usersWithRoles);




        //            @model IEnumerable<MVC5Demo.UI.ViewModels.Users_in_Role_ViewModel>

        //@{
        //                ViewBag.Title = "Users With Roles";
        //                Layout = "~/Views/Shared/_Layout.cshtml";
        //            }


        //< div class="panel panel-primary">  
        //    <div class="panel-heading">  
        //        <h3 class="box-title">  
        //            <b>Users with Roles</b>  
        //        </h3>  
        //    </div>  
        //    <!-- /.box-header -->  
        //    <div class="panel-body">  
        //        <table class="table table-hover table-bordered table-condensed" id="UsersWithRoles">  
        //            <thead>  
        //                <tr>  
        //                    <td><b>Username</b></td>  
        //                    <td><b>Email</b></td>  
        //                    <td><b>Roles</b></td>  
        //                </tr>  
        //            </thead>  
        //            @foreach(var user in Model)
        //        {
        //                < tr >
        //                    < td > @user.Username </ td >
        //                    < td > @user.Email </ td >
        //                    < td > @user.Role </ td >


        //                </ tr >
        //            }  
        //        </table>  
        //    </div>  

        //    <div class="panel-footer">  
        //        <p class="box-title"><b>Total Users till @String.Format("{0 : dddd, MMMM d, yyyy}", DateTime.Now)  : </b><span class="label label-primary">@Model.Count()</span></p>  
        //    </div>  
        //</div>  


        //@section scripts
        //        {  
        //    <script>  
        //        $(function () {  
        //            $('#UsersWithRoles').DataTable({
        //            "paging": true,  
        //                "lengthChange": true,  
        //                "searching": true,  
        //                "ordering": true,  
        //                "info": true,  
        //                "autoWidth": true
        //            });  
        //        });  
        //    </script>  
        //}  
        //}



    }
}
