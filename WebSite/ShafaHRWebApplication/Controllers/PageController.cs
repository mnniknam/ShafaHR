using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;

namespace ShafaHRWebApplication.Controllers
{
    public class PageController : BaseController
    {
        [AuthorizeAdmin]
        public IActionResult Index()
        {
            return View(db.NotDeleted<Page>());
        }

        [AuthorizeAdmin]
        public IActionResult Edit(long? Id)
        {
            if (Id == null || !long.TryParse(Id.ToString(), out long Id_Long)) return BadRequest();
            Page page_Exist = db.NotDeleted<Page>().FirstOrDefault(u => u.Id == Id);
            if (page_Exist == null) return NotFound();
            return View(page_Exist);
        }


        [AuthorizeAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page? page_Posted, IFormCollection form_Posted)
        {

            if (page_Posted.Id == null || !long.TryParse(page_Posted.Id.ToString(), out long Id_Long)) return BadRequest();
            Page page_Exist = db.NotDeleted<Page>().FirstOrDefault(u => u.Id == page_Posted.Id);
            if (page_Exist == null) return NotFound();

            page_Exist.Title = page_Posted.Title;
            page_Exist.BodyHTML = form_Posted["area"].ToString().Replace("../..", "");


            db.Entry(page_Exist).State = EntityState.Modified;
            page_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);
            await db.SaveChangesAsync();
            ViewBag.Message = "ثبت با موفقیت انجام شد.";

            return View(page_Exist);
        }

        public IActionResult Details(string? Code)
        {
            if (string.IsNullOrEmpty(Code)) return BadRequest();
            Page page_Exist = db.NotDeleted<Page>().FirstOrDefault(u => u.Code == (EnumPageCode)Enum.Parse(typeof(EnumPageCode), Code));
            if (page_Exist == null) return NotFound();
            return View(page_Exist);
        }


        [HttpPost]
        public async Task<IActionResult> Image(IFormFile file)
        {
            if (file == null || file.Length == 0 || file.Length > 10000000)
                return BadRequest();

            if (Path.GetExtension(file.FileName) != ".jpg" && Path.GetExtension(file.FileName) != ".jpeg" && Path.GetExtension(file.FileName) != ".gif" && Path.GetExtension(file.FileName) != ".png" && Path.GetExtension(file.FileName) != ".webp" && Path.GetExtension(file.FileName) != ".pdf" && Path.GetExtension(file.FileName) != ".doc" && Path.GetExtension(file.FileName) != ".docx")
                return BadRequest();

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Json(new { location = "/uploads/" + fileName });
        }


        [HttpPost]
        public async Task<IActionResult> File(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest();

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp", ".pdf", ".gif", ".doc", ".docx" };
            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!allowed.Contains(ext))
                return BadRequest("قالب فایل صحیح نیست");

            if (file.Length > 10000000)
                return BadRequest("حجم فایل بیشتر از 10 مگ است");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            var fileName = Guid.NewGuid() + ext;
            var path = Path.Combine(uploads, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return Json(new { location = "/uploads/" + fileName });
        }
    }
}
