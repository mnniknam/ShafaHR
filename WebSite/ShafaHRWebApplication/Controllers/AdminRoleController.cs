using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;

namespace ShafaHRWebApplication.Controllers
{
    [AuthorizeAdmin]
    public class AdminRoleController : BaseController
    {
        // GET: AdminRole

        public ActionResult Index(int? AdminId)
        {
            if (AdminId == null)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.AdminId != AdminId)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            Admin admin_Exist = db.NotDeleted<Admin>().Where(u => u.Id == AdminId).FirstOrDefault();

            if (admin_Exist == null) { return RedirectToAction("NotFound", "Error"); }

            ViewBag.AdminId = AdminId;
            ViewBag.AdminFullName = admin_Exist.Firstname + ' ' + admin_Exist.Lastname;

            return View(admin_Exist.AdminRole?.Where(u => u.RecordDeleted == false).ToList());
        }

        public ActionResult Create(int? Id)
        {

            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            Admin admin_Exist = db.NotDeleted<Admin>().Where(u => u.Id == Id).FirstOrDefault();

            if (admin_Exist == null) { return RedirectToAction("NotFound", "Error"); }

            ViewBag.AdminId = Id;
            ViewBag.AdminFullName = admin_Exist.Firstname + ' ' + admin_Exist.Lastname;

            AdminRole adminRole_New = new AdminRole() { AdminId = admin_Exist.Id };

            return View(adminRole_New);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminRole adminRole)
        {
            if (adminRole.Id == 0)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            Admin admin_Exist = db.NotDeleted<Admin>().Where(u => u.Id == adminRole.AdminId).FirstOrDefault();

            if (admin_Exist == null) { return RedirectToAction("NotFound", "Error"); }


            ViewBag.AdminFullName = admin_Exist.Firstname + ' ' + admin_Exist.Lastname;

            SelectListItem s = new SelectListItem
            {
                Text = "-- بیمارستان --",
                Value = "",
                Selected = true
            };

            if (admin_Exist.AdminRole.Where(u => u.RecordDeleted == false && u.Role == adminRole.Role).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", new { id = admin_Exist.Id });
            }


            AdminRole adminRole_New = new AdminRole
            {
                AdminId = admin_Exist.Id,
                Role = adminRole.Role

            };

            db.AdminRole.Add(adminRole_New);
            adminRole_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);


            db.SaveChanges();

            return RedirectToAction("Index", new { AdminId = admin_Exist.Id });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsDefault(int? roleId, bool? IsDefault, int? adminId)
        {

            if (roleId == null || IsDefault == null || adminId == null)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.AdminId != adminId)
            {
                return RedirectToAction("NotAccess", "Error");
            }

            Admin admin_Exist = db.Admin.Where(u => u.RecordDeleted == false && u.Id == adminId).FirstOrDefault();

            if (admin_Exist == null) { return RedirectToAction("NotFound", "Error"); }

            if (admin_Exist.AdminRole == null) { return RedirectToAction("NotAccess", "Error"); }


            AdminRole adminRole_Exist = admin_Exist.AdminRole.Where(u => u.RecordDeleted == false && u.Id == roleId).FirstOrDefault();

            if (adminRole_Exist == null) { return RedirectToAction("NotAccess", "Error"); }


            adminRole_Exist.IsDefault = (bool)IsDefault;

            db.Entry(adminRole_Exist).State = EntityState.Modified;

            if (admin_Exist.AdminRole.Count > 1)
            {
                foreach (AdminRole UR in admin_Exist.AdminRole)
                {
                    if (UR.Id != roleId)
                    {
                        UR.IsDefault = false;
                        db.Entry(UR).State = EntityState.Modified;
                    }
                }
            }

            db.SaveChanges();

            return RedirectToAction("Index", new { AdminId = admin_Exist.Id });
        }


        [AuthorizeAdmin]
        public async Task<ActionResult> Delete(long? Id)
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin || Id == null || !long.TryParse(Id.ToString(), out long Id_Long))
            {
                return RedirectToAction("Dashboard", "Home");
            }

            AdminRole adminRole_Exist = db.NotDeleted<AdminRole>().Where(u => u.Id == Id).FirstOrDefault();

            if (adminRole_Exist == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            adminRole_Exist.RecordDeleted = true;

            db.Entry(adminRole_Exist).State = EntityState.Modified;

            adminRole_Exist.SetRecordDataForDelete(db, AuthorizationAdmin.Current.AdminId);

            await db.SaveChangesAsync();

            return RedirectToAction("Index", new { adminRole_Exist.AdminId });
        }
    }
}
