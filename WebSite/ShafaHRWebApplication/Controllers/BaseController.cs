using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShafaHRWebApplication.Controllers
{
    public class BaseController : Controller
    {
        public EFContext db = new EFContext(new DbContextOptionsBuilder<EFContext>().UseLazyLoadingProxies().UseSqlServer(CommonFunctions.ConnectionString).Options);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //ViewBag.ManufacturersCount = db.NotDeleted<CentersStep>().Where(u => u.Centers is Manufacturers && u.Status == EnumCentersStepStatus.Evaluation && u.DoDate == null)?.Count();
            //ViewBag.CompanyCount = db.NotDeleted<CentersStep>().Where(u => u.Centers is Company && u.Status == EnumCentersStepStatus.Evaluation && u.DoDate == null)?.Count();
            //ViewBag.MedicalCentersCount = db.NotDeleted<CentersStep>().Where(u => u.Centers is MedicalCenters && u.Status == EnumCentersStepStatus.Evaluation && u.DoDate == null)?.Count();
            base.OnActionExecuting(context);
        }
    }
}
