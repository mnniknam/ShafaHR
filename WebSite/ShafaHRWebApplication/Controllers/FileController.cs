using Microsoft.AspNetCore.Mvc;
using ShafaHRCoreLib.Attributes;
using ShafaHRCoreLib.Managers;

namespace ShafaHRWebApplication.Controllers
{
    public class FileController : BaseController
    {
        // GET: File
        [AuthorizeAdmin]
        public IActionResult Index(long? Id)
        {
            string strId = Id.ToString();
            var fileToRetrieve = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(f => f.Id.ToString() == strId).FirstOrDefault();
            return File(System.IO.File.ReadAllBytes(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName((DateTime)fileToRetrieve.RecordCreated), "\\", fileToRetrieve.Id, fileToRetrieve.Extension)), fileToRetrieve.ContentType);
        }

        public IActionResult ShowFile(long? Id)
        {
            string strId = Id.ToString();
            var fileToRetrieve = db.NotDeleted<ShafaHRCoreLib.Models.File>().Where(f => f.Id.ToString() == strId).FirstOrDefault();
            return File(System.IO.File.ReadAllBytes(String.Concat(CommonFunctions.FileFolderPath, "\\", CommonFunctions.GetDateDirectoryName((DateTime)fileToRetrieve.RecordCreated), "\\", fileToRetrieve.Id, fileToRetrieve.Extension)), fileToRetrieve.ContentType);
        }
    }
}
