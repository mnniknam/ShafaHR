using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;

namespace ShafaHRWebApplication.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            { return RedirectToAction("NotAccess", "Error"); }

            var roles = AuthorizationAdmin.Current.RoleS.ToList();

            List<SelectListItem> slItems = new List<SelectListItem>();

            AdminRole SelectedObject = null;

            foreach (AdminRole ur in roles)
            {

                SelectListItem slItem = new SelectListItem
                {
                    Selected = false
                };

                slItem.Text = CommonFunctions.GetDisplayName(ur.Role);

                if (ur.IsDefault) { SelectedObject = ur; slItem.Selected = true; }

                slItem.Value = ur.Id.ToString();
                slItems.Add(slItem);

            }


            if (SelectedObject != null)
            {
                ViewBag.Roles = new SelectList(slItems, "Value", "Text", SelectedObject.Id);
            }
            else
            {
                ViewBag.Roles = new SelectList(slItems, "Value", "Text");
            }

            return View(db.Admin.Where(u => u.RecordDeleted == false).OrderByDescending(r => r.Id).ToList());
        }


        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }

            if (id != AuthorizationAdmin.Current.AdminId && AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {

                return RedirectToAction("NotAccess", "Error");

            }

            Admin adminInfo_Exist = db.NotDeleted<Admin>().Where(u => u.Id == id).FirstOrDefault();

            if (adminInfo_Exist == null)
            {
                return RedirectToAction("NotFound", "Error");
            }

            return View(adminInfo_Exist);

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id != AuthorizationAdmin.Current.AdminId && AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            Admin? admin_Exist = db.NotDeleted<Admin>().Where(u => u.Id == id).FirstOrDefault();

            if (admin_Exist == null)
            {
                return NotFound();
            }


            return View(admin_Exist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
        {
            if (admin.Id != AuthorizationAdmin.Current.AdminId && AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            Admin? admin_Exist = db.NotDeleted<Admin>().Where(u => u.Id == admin.Id).FirstOrDefault();

            if (admin_Exist == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                //if (admin.Mobile != null && admin.Mobile != "" && db.Admin.Where(r => r.Mobile == admin.Mobile && r.Id != admin.Id && r.RecordDeleted == false).FirstOrDefault() != null)
                //{
                //    ViewBag.Message = "ثبت نام ناموفق! شماره همراه تکراری است.";
                //    return View(admin);
                //}

                if (admin.UserName != null && admin.UserName != "" && db.Admin.Where(r => r.Mobile == admin.UserName && r.Id != admin.Id && r.RecordDeleted == false).FirstOrDefault() != null)
                {
                    ViewBag.Message = "ثبت نام ناموفق! نام کاربری تکراری است.";
                    return View(admin);
                }



                admin_Exist.UserName = admin.UserName;
                admin_Exist.Firstname = admin.Firstname;
                admin_Exist.Lastname = admin.Lastname;
                admin_Exist.Mobile = admin.Mobile;

                db.Entry(admin_Exist).State = EntityState.Modified;

                admin_Exist.SetRecordDataForModify(db, admin.Id);

                db.SaveChanges();


                if (admin.Id == AuthorizationAdmin.Current.AdminId)
                {

                    AuthorizationAdmin.Current.Firstname = admin.Firstname;
                    AuthorizationAdmin.Current.Lastname = admin.Lastname;

                }

                return RedirectToAction("Index");
            }

            return View(admin);
        }

        public ActionResult Create()
        {

            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            if (ModelState.IsValid)
            {

                if (admin.UserName != null && db.NotDeleted<Admin>().Where(r => r.UserName.ToLower() == admin.UserName.Trim().ToLower()).FirstOrDefault() != null)
                {
                    ViewBag.Message = "ثبت نام ناموفق! نام کاربری تکراری است.";
                    return View(admin);
                }

                Admin admin_New = new Admin
                {
                    Firstname = admin.Firstname,
                    Lastname = admin.Lastname,
                    UserName = admin.UserName.Trim().ToLower(),
                    Mobile = admin.Mobile,
                    PasswordHash = CommonFunctions.GetSHA1(admin.PasswordHash)
                };

                db.Admin.Add(admin_New);
                db.SaveChanges();

                admin_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(admin);
            }

        }
    }
}
