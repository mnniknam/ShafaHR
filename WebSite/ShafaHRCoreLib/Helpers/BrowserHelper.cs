using UAParser;

namespace ShafaHRCoreLib.Helpers
{
    public static class BrowserHelper
    {
        // شبیه HttpContext.Current.Request.Browser.Id
        public static string GetBrowserId()
        {
            var context = HttpContextProvider.Current; // کلاس HttpContextProvider که قبلاً ساختیم
            if (context == null)
                return "Unknown";

            var userAgent = context.Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";

            var parser = Parser.GetDefault();
            var client = parser.Parse(userAgent);

            // شناسه ساده: نام مرورگر + نسخه اصلی
            return $"{client.UA.Family}_{client.UA.Major}";
        }

        // متد کامل برای اطلاعات مرورگر (اختیاری)
        public static string GetBrowserInfo()
        {
            var context = HttpContextProvider.Current;
            if (context == null)
                return "Unknown";

            var userAgent = context.Request.Headers["User-Agent"].ToString();
            if (string.IsNullOrEmpty(userAgent))
                return "Unknown";

            var parser = Parser.GetDefault();
            var client = parser.Parse(userAgent);

            return $"Browser: {client.UA.Family} {client.UA.Major}.{client.UA.Minor}, OS: {client.OS.Family} {client.OS.Major}.{client.OS.Minor}, Device: {client.Device.Family}";
        }
    }
}
