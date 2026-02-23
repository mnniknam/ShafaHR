using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;

namespace ShafaHRWebApplication.Controllers
{
    public class PublicationController : BaseController
    {
        private readonly string _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", "Temp");

        [AuthorizeAdmin]
        public IActionResult Index()
        {
            return View(db.NotDeleted<Publication>().OrderByDescending(u => u.Id).ToList());
        }

        public IActionResult List()
        {
            return View(db.NotDeleted<Publication>().Where(u => u.Status == EnumPublicationStatus.Published).OrderByDescending(u => u.Id).ToList());
        }

        [AuthorizeAdmin]
        public IActionResult Create()
        {
            Publication publication_New = new();
            return View(publication_New);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public async Task<IActionResult> Create(Publication publication_Posted)
        {
            if (string.IsNullOrEmpty(publication_Posted.Title))
            {
                ModelState.AddModelError(string.Empty, "عنوان را وارد کنید."); return View(publication_Posted);
            }

            if (string.IsNullOrEmpty(publication_Posted.Summary))
            {
                ModelState.AddModelError(string.Empty, "خلاصه را وارد کنید."); return View(publication_Posted);
            }

            if (publication_Posted.Category == null || publication_Posted.Category <= 0)
            {
                ModelState.AddModelError(string.Empty, "دسته بندی را وارد کنید."); return View(publication_Posted);
            }

            if (ModelState.IsValid)
            {

                Publication publication_New = new Publication
                {
                    Title = CommonFunctions.ConvertArabicToFarsi(publication_Posted.Title),
                    Summary = CommonFunctions.ConvertArabicToFarsi(publication_Posted.Summary),
                    Category = publication_Posted.Category
                };

                db.Publication.Add(publication_New);
                publication_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);

                await db.SaveChangesAsync();


                return RedirectToAction("Edit", new { publication_New.Id });

            }
            else { ModelState.AddModelError(string.Empty, "اشکال در ثبت."); return View(publication_Posted); }


        }


        [AuthorizeAdmin]
        public IActionResult Edit(long? Id)
        {
            Publication publication_Exist = db.NotDeleted<Publication>().FirstOrDefault(u => u.Id == Id);
            return View(publication_Exist);
        }

        [HttpPost]
        [AuthorizeAdmin]
        public async Task<ActionResult> Edit(Publication publication_Posted, IFormFile? File)
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                ModelState.AddModelError(string.Empty, "مجوز دسترسی ندارید.");
                return View(publication_Posted);
            }

            if (publication_Posted.Id == 0 || !long.TryParse(publication_Posted.Id.ToString(), out long Id_Long))
            {
                return RedirectToAction("Index");
            }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == publication_Posted.Id).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Posted);
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(publication_Posted.Title))
                    publication_Exist.Title = CommonFunctions.ConvertArabicToFarsi(publication_Posted.Title.Trim());
                if (!string.IsNullOrEmpty(publication_Posted.Summary))
                    publication_Exist.Summary = CommonFunctions.ConvertArabicToFarsi(publication_Posted.Summary.Trim());
                if (!string.IsNullOrEmpty(publication_Posted.Body))
                    publication_Exist.Body = CommonFunctions.ConvertArabicToFarsi(publication_Posted.Body.Trim());
                publication_Exist.Status = publication_Posted.Status;
                publication_Exist.Category = publication_Posted.Category;

                //Thumbnail
                if (File != null)
                {
                    if (publication_Exist.ThumbnailId != null)
                    {
                        ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.ThumbnailId).FirstOrDefault();
                        file_Exist.RecordDeleted = true;
                        db.Entry(file_Exist).State = EntityState.Modified;
                    }

                    ShafaHRCoreLib.Models.File file_New = new ShafaHRCoreLib.Models.File
                    {
                        ContentType = File.ContentType,
                        Extension = System.IO.Path.GetExtension(File.FileName),
                        FileName = File.FileName
                    };

                    db.File.Add(file_New);

                    file_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);

                    publication_Exist.Thumbnail = file_New;
                }

                db.Entry(publication_Exist).State = EntityState.Modified;
                publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);
                await db.SaveChangesAsync();

                if (!System.IO.Directory.Exists(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now))))
                {
                    System.IO.Directory.CreateDirectory(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)));
                }

                if (File != null && File.Length > 0)
                {
                    var filePath = Path.Combine(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)), string.Concat(publication_Exist.ThumbnailId, publication_Exist.Thumbnail.Extension));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        File.CopyTo(stream);
                    }
                }

                ViewBag.Message = "ثبت با موفقیت انجام شد.";

                return RedirectToAction("Index");

            }
            else { ModelState.AddModelError(string.Empty, "اشکال در ثبت."); return View(publication_Exist); }

        }


        [AuthorizeAdmin]
        public async Task<ActionResult> Delete(long? Id)
        {

            if ((AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications) || Id == null || !long.TryParse(Id.ToString(), out long Id_Long))
            {
                return RedirectToAction("Dashboard", "Home");
            }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == Id).FirstOrDefault();

            if (publication_Exist == null)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            publication_Exist.RecordDeleted = true;

            db.Entry(publication_Exist).State = EntityState.Modified;

            publication_Exist.SetRecordDataForDelete(db, AuthorizationAdmin.Current.AdminId);

            await db.SaveChangesAsync();

            return RedirectToAction("Index");

        }


        [AuthorizeAdmin]
        public async Task<ActionResult> Upload(long? PublicationId, IFormFile File)
        {
            if (File == null) { return Ok(); }
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (PublicationId == null || !long.TryParse(PublicationId.ToString(), out long PublicationId_Long)) { return View(); }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == PublicationId).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Exist);
            }

            if (Path.GetExtension(File.FileName) != ".jpg" && Path.GetExtension(File.FileName) != ".jpeg" && Path.GetExtension(File.FileName) != ".git" && Path.GetExtension(File.FileName) != ".png" && Path.GetExtension(File.FileName) != ".webp")
            {
                ViewBag.Message = "ثبت ناموفق! قالب فایل صحیح نیست.";
                return View(publication_Exist);
            }

            if (File.Length > 10000000)
            {
                ViewBag.Message = "ثبت ناموفق! حداکثر حجم مجاز 10 مگا بایت است.";
                return View(publication_Exist);
            }

            if (publication_Exist.ThumbnailId != null)
            {
                ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.ThumbnailId).FirstOrDefault();
                file_Exist.RecordDeleted = true;
                db.Entry(file_Exist).State = EntityState.Modified;
            }

            ShafaHRCoreLib.Models.File file_New = new ShafaHRCoreLib.Models.File
            {
                ContentType = File.ContentType,
                Extension = System.IO.Path.GetExtension(File.FileName),
                FileName = File.FileName
            };

            db.File.Add(file_New);

            file_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);

            publication_Exist.Thumbnail = file_New;

            db.Entry(publication_Exist).State = EntityState.Modified;

            publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);


            await db.SaveChangesAsync();


            if (!System.IO.Directory.Exists(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now))))
            {
                System.IO.Directory.CreateDirectory(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)));
            }


            var filePath = Path.Combine(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)), string.Concat(file_New.Id, file_New.Extension));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                File.CopyTo(stream);
            }


            return Ok(new { message = "آپلود با موفقیت انجام شد." });

        }

        [AuthorizeAdmin]
        public async Task<ActionResult> UploadPDF(long? PublicationId, IFormFile File)
        {
            if (File == null) { return Ok(); }
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (PublicationId == null || !long.TryParse(PublicationId.ToString(), out long PublicationId_Long)) { return View(); }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == PublicationId).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Exist);
            }

            if (Path.GetExtension(File.FileName) != ".pdf")
            {
                ViewBag.Message = "ثبت ناموفق! قالب فایل صحیح نیست.";
                return View(publication_Exist);
            }

            if (File.Length > 30000000)
            {
                ViewBag.Message = "ثبت ناموفق! حداکثر حجم مجاز 30 مگا بایت است.";
                return View(publication_Exist);
            }

            if (publication_Exist.PDFId != null)
            {
                ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.ThumbnailId).FirstOrDefault();
                file_Exist.RecordDeleted = true;
                db.Entry(file_Exist).State = EntityState.Modified;
            }

            ShafaHRCoreLib.Models.File file_New = new ShafaHRCoreLib.Models.File
            {
                ContentType = File.ContentType,
                Extension = System.IO.Path.GetExtension(File.FileName),
                FileName = File.FileName
            };

            db.File.Add(file_New);

            file_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);

            publication_Exist.PDF = file_New;

            db.Entry(publication_Exist).State = EntityState.Modified;

            publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);


            await db.SaveChangesAsync();


            if (!System.IO.Directory.Exists(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now))))
            {
                System.IO.Directory.CreateDirectory(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)));
            }


            var filePath = Path.Combine(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)), string.Concat(file_New.Id, file_New.Extension));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                File.CopyTo(stream);
            }


            return Ok(new { message = "آپلود با موفقیت انجام شد." });

        }


        [AuthorizeAdmin]
        public async Task<ActionResult> ThumbnailDelete(long? Id)
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (Id == null || !long.TryParse(Id.ToString(), out long PublicationId_Long)) { return View(); }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == Id).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Exist);
            }

            if (publication_Exist.ThumbnailId != null)
            {
                ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.ThumbnailId).FirstOrDefault();
                file_Exist.RecordDeleted = true;
                db.Entry(file_Exist).State = EntityState.Modified;
                file_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);
            }

            publication_Exist.ThumbnailId = null;

            db.Entry(publication_Exist).State = EntityState.Modified;

            publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);

            await db.SaveChangesAsync();

            return RedirectToAction("Edit", new { publication_Exist.Id });

        }

        [AuthorizeAdmin]
        public async Task<ActionResult> PDFDelete(long? Id)
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (Id == null || !long.TryParse(Id.ToString(), out long PublicationId_Long)) { return View(); }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == Id).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Exist);
            }

            if (publication_Exist.PDFId != null)
            {
                ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.PDFId).FirstOrDefault();
                file_Exist.RecordDeleted = true;
                db.Entry(file_Exist).State = EntityState.Modified;
                file_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);
            }

            publication_Exist.PDFId = null;

            db.Entry(publication_Exist).State = EntityState.Modified;

            publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);

            await db.SaveChangesAsync();

            return RedirectToAction("Edit", new { publication_Exist.Id });

        }

        [AuthorizeAdmin]
        public async Task<ActionResult> VideoDelete(long? Id)
        {
            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (Id == null || !long.TryParse(Id.ToString(), out long PublicationId_Long)) { return View(); }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == Id).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Exist);
            }

            if (publication_Exist.VideoId != null)
            {
                ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.VideoId).FirstOrDefault();
                file_Exist.RecordDeleted = true;
                db.Entry(file_Exist).State = EntityState.Modified;
            }

            publication_Exist.VideoId = null;

            db.Entry(publication_Exist).State = EntityState.Modified;

            publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);

            await db.SaveChangesAsync();

            return RedirectToAction("Edit", new { publication_Exist.Id });

        }

        public ActionResult Details(long? Id)
        {
            if (Id == null || !long.TryParse(Id.ToString(), out long Id_Long))
            {
                return RedirectToAction("Index", "Home");
            }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == Id && u.Status == EnumPublicationStatus.Published).FirstOrDefault();

            if (publication_Exist == null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View(publication_Exist);

        }

        public async Task<IActionResult> UploadChunk(IFormFile File, string fileName, int chunkIndex, int totalChunks, long PublicationId)
        {
            if (File == null) { return Ok(); }

            if (AuthorizationAdmin.Current.Role != EnumAdminRole.SysAdmin && AuthorizationAdmin.Current.Role != EnumAdminRole.EditorPublications)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (PublicationId == null || !long.TryParse(PublicationId.ToString(), out long PublicationId_Long)) { return View(); }

            Publication publication_Exist = db.NotDeleted<Publication>().Where(u => u.Id == PublicationId).FirstOrDefault();

            if (publication_Exist == null)
            {
                ViewBag.Message = "ثبت ناموفق! انتشارات یافت نشد.";
                return View(publication_Exist);
            }

            if (Path.GetExtension(fileName) != ".mp4")
            {
                ViewBag.Message = "ثبت ناموفق! فایل MP4 انتخاب کنید.";
                return View(publication_Exist);
            }

            if (File.Length > 500000000)
            {
                ViewBag.Message = "ثبت ناموفق! حداکثر حجم مجاز 500 مگا بایت است.";
                return View(publication_Exist);
            }

            if (publication_Exist.VideoId != null)
            {
                ShafaHRCoreLib.Models.File file_Exist = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(u => u.Id == publication_Exist.VideoId).FirstOrDefault();
                file_Exist.RecordDeleted = true;
                db.Entry(file_Exist).State = EntityState.Modified;
            }

            //Directory.CreateDirectory(_tempPath);

            var chunkFilePath = Path.Combine(_tempPath, $"{fileName}.part_{chunkIndex}");

            using (var stream = new FileStream(chunkFilePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            // اگر آخرین chunk بود → merge
            if (chunkIndex == totalChunks - 1)
            {
                ShafaHRCoreLib.Models.File file_New = new ShafaHRCoreLib.Models.File
                {
                    ContentType = "video/mp4",
                    Extension = Path.GetExtension(fileName),
                    FileName = fileName
                };

                db.File.Add(file_New);

                file_New.SetRecordDataForCreate(db, AuthorizationAdmin.Current.AdminId);

                publication_Exist.Video = file_New;

                db.Entry(publication_Exist).State = EntityState.Modified;

                publication_Exist.SetRecordDataForModify(db, AuthorizationAdmin.Current.AdminId);


                await db.SaveChangesAsync();

                await MergeChunks(fileName, totalChunks, string.Concat(file_New.Id, ".mp4"));


            }



            return Ok(new { message = "آپلود با موفقیت انجام شد." });

        }

        private async Task MergeChunks(string fileName, int totalChunks, string NewFileName)
        {

            if (!System.IO.Directory.Exists(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now))))
            {
                System.IO.Directory.CreateDirectory(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)));
            }


            var finalPath = Path.Combine(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName(DateTime.Now)), NewFileName);


            using var finalStream = new FileStream(finalPath, FileMode.Create);

            for (int i = 0; i < totalChunks; i++)
            {

                var partPath = Path.Combine(_tempPath, $"{fileName}.part_{i}");

                try
                {
                    using (var partStream = new FileStream(partPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        await partStream.CopyToAsync(finalStream);
                    }
                    System.IO.File.Delete(partPath);
                    //using var partStream = new FileStream(partPath, FileMode.Open);
                    //await partStream.CopyToAsync(finalStream);
                }
                catch (Exception ex) { }


                //System.IO.File.Delete(partPath);
            }


        }
    }
}
