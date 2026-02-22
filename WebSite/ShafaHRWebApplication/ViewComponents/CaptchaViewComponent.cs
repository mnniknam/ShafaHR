using Microsoft.AspNetCore.Mvc;
using ShafaHRCoreLib.Helpers;
using System.Drawing;
using System.Drawing.Imaging;

namespace ShafaHRWebApplication.ViewComponents
{
    public class CaptchaViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var code = GenerateCaptchaCode();
            var context = HttpContextProvider.Current;
            context.Session.SetString("CaptchaCode", code);

            var imageBase64 = GenerateCaptchaImageBase64(code);

            return View("Default", imageBase64);
        }

        private string GenerateCaptchaCode(int length = 5)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GenerateCaptchaImageBase64(string code)
        {
            using var bmp = new Bitmap(110, 30);
            using var gfx = Graphics.FromImage(bmp);
            gfx.Clear(Color.LightGray);
            using var font = new Font("Arial", 20, FontStyle.Bold);
            gfx.DrawString(code, font, Brushes.Black, new PointF(1, 3));
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
