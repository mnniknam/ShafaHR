using Microsoft.AspNetCore.Mvc;

namespace ShafaHRWebApplication.Controllers
{
    public class CaptchaController : Controller
    {
        public IActionResult Reload()
        {
            return ViewComponent("Captcha");
        }
    }
}
