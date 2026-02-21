using Microsoft.AspNetCore.Http;

namespace ShafaHRCoreLib.Helpers
{
    public static class HttpContextProvider
    {
        private static IHttpContextAccessor _accessor;

        public static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static HttpContext Current => _accessor?.HttpContext;
    }
}
