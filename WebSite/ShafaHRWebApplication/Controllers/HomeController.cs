using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;
using ShafaHRWebApplication.Models;
using System.Diagnostics;

namespace ShafaHRWebApplication.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
           
            ViewBag.PublicationList = db.NotDeleted<Publication>().Where(u => u.Status == EnumPublicationStatus.Published).OrderByDescending(u => u.Id).Take(10).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AuthorizeAdmin]
        public IActionResult Dashboard()
        {
            return View();
        }


        public ActionResult NavBar()
        {
          return PartialView();
        }

        [AuthorizeAdmin]
        public ActionResult SelectUserRoles()
        {

            var adminRoles = db.NotDeleted<AdminRole>().Where(u => u.AdminId == AuthorizationAdmin.Current.AdminId).ToList();

            if (adminRoles != null)
            {
                List<SelectListItem> slItems = new List<SelectListItem>();

                AdminRole SelectedObject = null;

                foreach (AdminRole ur in adminRoles)
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
                    SelectList sl = new SelectList(slItems, "Value", "Text", SelectedObject.Id); return Ok(sl);
                }
                else
                {
                    SelectList sl = new SelectList(slItems, "Value", "Text");
                    return Ok(sl);
                }

            }

            return null;
        }

        public ActionResult SetRole(int? id)
        {
            //if (AuthorizationAdmin.Current.Admin.AdminRole?.Where(u => u.RecordDeleted == false && u.Id == id).FirstOrDefault() == null)
            //{
            //    return Json("success");
            //}

            AdminRole? urExist = db.AdminRole.Where(u => u.Id == id && u.RecordDeleted != true).FirstOrDefault();

            if (urExist != null)
            {
                AuthorizationAdmin.Current.Role = urExist.Role;

                urExist.IsDefault = true;
                db.Entry(urExist).State = EntityState.Modified;

            }

            var query = from ur in db.AdminRole where ur.AdminId == urExist.AdminId && ur.Id != id select ur;

            foreach (AdminRole ur in query)
            {
                ur.IsDefault = false;
                db.Entry(ur).State = EntityState.Modified;
            }

            db.SaveChanges();

            return Json("success");

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
