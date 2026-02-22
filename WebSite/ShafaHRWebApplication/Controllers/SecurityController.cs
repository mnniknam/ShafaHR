using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShafaHRWebApplication.Controllers
{
    public class SecurityController : BaseController
    {
        public object JsonRequestBehavior { get; private set; }

        public IActionResult Index()
        {
            return View();
        }


       
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            
            Admin? admin_Exist = db.NotDeleted<Admin>().Where(u => u.UserName == "admin").FirstOrDefault();

            if (admin_Exist == null)
            {
                Admin admin_New = new Admin
                {
                    Firstname = "ادمین",
                    Lastname = "سامانه",
                    UserName = "admin",
                    PasswordHash = CommonFunctions.GetSHA1("1234"),

                };

                db.Admin.Add(admin_New);

                AdminRole adminRole_New = new AdminRole
                {
                    Role = EnumAdminRole.SysAdmin,
                    Admin = admin_New,
                    IsDefault = true
                };

                db.AdminRole.Add(adminRole_New);

                await db.SaveChangesAsync();
            }
            else
            {
                if (admin_Exist.AdminRole.Where(r => r.RecordDeleted == false && r.Role == EnumAdminRole.SysAdmin).FirstOrDefault() == null)
                {
                    AdminRole adminRole_New = new AdminRole
                    {
                        Role = EnumAdminRole.SysAdmin,
                        Admin = admin_Exist
                    };

                    await db.SaveChangesAsync();
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel LoginAdmin, string CaptchaInput)
        {
            var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");

            if (CaptchaInput.ToUpper() != sessionCaptcha)
            {
                ModelState.AddModelError("CaptchaInput", "متن اشتباه است");
                return View();
            }

            string LoginPassword = CommonFunctions.GetSHA1(CommonFunctions.GetEnglishNumber(LoginAdmin.Password));

            Admin? ui = db.NotDeleted<Admin>().Where(u => u.UserName == LoginAdmin.UserName.Trim() && u.PasswordHash == LoginPassword).FirstOrDefault();

            if (ui != null)
            {
                AuthorizationAdmin.Current.DoAuthenticatedAndHandleRoles(ui);

                return RedirectToAction("Dashboard", "Home");

            }
            else
            {
                ViewBag.message = "ورود ناموفق.";
            }

            return View();
        }

        public ActionResult Logout()
        {
            AuthorizationAdmin.Current.DoLogout();
            return RedirectToAction("Dashboard", "Home");
        }

        [AuthorizeAdmin]
        public ActionResult Setting()
        {
            var options = new DbContextOptionsBuilder<EFContext>().UseLazyLoadingProxies().UseSqlServer(CommonFunctions.ConnectionString).Options;
            EFContext? db = new EFContext(options);

            Admin? admin_Exist = db.NotDeleted<Admin>().FirstOrDefault(u => u.Id == AuthorizationAdmin.Current.AdminId);

            if (admin_Exist == null)
            {
                return NotFound();
            }

            return View(admin_Exist);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [AuthorizeAdmin]
        [HttpPost]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                Admin? adminInfo = db.NotDeleted<Admin>().FirstOrDefault(u => u.Id == AuthorizationAdmin.Current.AdminId);

                if (adminInfo == null)
                {
                    ViewBag.Message = "اطلاعات کاربری صحیح نیست.";
                    return View(changePasswordViewModel);
                }

                if (changePasswordViewModel.NewPassword.Trim() != changePasswordViewModel.ConfirmPassword.Trim())
                {
                    ViewBag.Message = "کلمه عبور یکسان نیست.";
                    return View(changePasswordViewModel);
                }

                if (adminInfo.PasswordHash == CommonFunctions.GetSHA1(changePasswordViewModel.OldPassword.Trim()))
                {
                    adminInfo.PasswordHash = CommonFunctions.GetSHA1(changePasswordViewModel.NewPassword.Trim());

                    await db.SaveChangesAsync();
                    ViewBag.Message = "کلمه عبور با موفقیت تغییر یافت.";
                    return View(changePasswordViewModel);
                }

            }
            return View(changePasswordViewModel);
        }

       

        
    }
}
