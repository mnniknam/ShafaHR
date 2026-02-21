using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace ShafaHRCoreLib.Attributes
{
    public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // ================================
            // 🔴 بخش اول — جایگزین HandleUnauthorizedRequest برای کاربر لاگین نشده
            // ================================
            if (!Managers.AuthorizationAdmin.Current.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new { controller = "Security", action = "Login" }
                    )
                );
                return;
            }

            // ================================
            // 🔵 بخش دوم — جایگزین HandleUnauthorizedRequest برای عدم دسترسی
            // ================================
            //if (!string.IsNullOrEmpty(Roles))
            //{
            //    var roles = Roles.Split(',').Select(r => r.Trim());

            //    if (!roles.Any(role => user.IsInRole(role)))
            //    {
            //        context.Result = new RedirectToRouteResult(
            //            new RouteValueDictionary(
            //                new { controller = "Account", action = "AccessDenied" }
            //            )
            //        );
            //        return;
            //    }
            //}

            // اگر همه چیز OK بود → دسترسی داده می‌شود و اکشن ادامه پیدا می‌کند
        }
    }
}
