using ShafaHRCoreLib.Helpers;
using ShafaHRCoreLib.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ShafaHRCoreLib.Managers
{
    public class AuthorizationAdmin
    {
        public static AuthorizationAdmin Current
        {
            get
            {
                var context = HttpContextProvider.Current; // مشابه HttpContext.Current

                var auth = context.Request.Cookies["IranAtmp"];

                if (string.IsNullOrEmpty(auth))
                {
                    return new AuthorizationAdmin();
                }
                else
                {
                    var options = new DbContextOptionsBuilder<EFContext>().UseSqlServer(CommonFunctions.ConnectionString).Options;
                    EFContext db = new EFContext(options);

                    AuthorizationAdmin authorizationAdmin_New = new AuthorizationAdmin();
                    authorizationAdmin_New.boolIsAuthenticated = true;
                    string[] m = auth.Split('-');

                    if (m.Length != 3 || CommonFunctions.EncryptWord(BrowserHelper.GetBrowserId()) != m[2]) return new AuthorizationAdmin();

                    DateTime dt = Convert.ToDateTime(CommonFunctions.DecryptWord(m[1]));

                    if (dt < DateTime.Now) return new AuthorizationAdmin();

                    long adminId = Convert.ToInt64(CommonFunctions.DecryptWord(m[0]));

                    Admin? admin_Exist = db.NotDeleted<Admin>().FirstOrDefault(u => u.Id == adminId);
                    List<AdminRole> adminRole_list = db.NotDeleted<AdminRole>().Where(u => u.AdminId == admin_Exist.Id).ToList();
                    if (adminRole_list.Count > 0)
                    {
                        authorizationAdmin_New.RoleS = adminRole_list;
                        authorizationAdmin_New.Role = adminRole_list.FirstOrDefault().Role;
                    }


                    if (admin_Exist != null)
                    {
                        db.Entry(admin_Exist).Collection("AdminRole").LoadAsync();
                        authorizationAdmin_New.AdminId = admin_Exist.Id;
                        authorizationAdmin_New.Firstname = admin_Exist.Firstname;
                        authorizationAdmin_New.Lastname = admin_Exist.Lastname;

                        if (admin_Exist.AdminRole != null && admin_Exist.AdminRole.Count > 0)
                        {
                            if (admin_Exist.AdminRole.FirstOrDefault(u => u.RecordDeleted == false && u.IsDefault == true) != null)
                                authorizationAdmin_New.Role = admin_Exist.AdminRole.FirstOrDefault(u => u.RecordDeleted == false && u.IsDefault == true).Role;
                            else authorizationAdmin_New.Role = admin_Exist.AdminRole.FirstOrDefault(u => u.RecordDeleted == false).Role;


                        }
                    }


                    return authorizationAdmin_New;
                }
            }
        }
        private bool boolIsAuthenticated;

        public long AdminId { get; set; }
        public EnumAdminRole Role { get; set; }
        public ICollection<AdminRole>? RoleS { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        public bool IsAuthenticated
        {
            get
            {
                if (boolIsAuthenticated)
                    return true;
                else
                    return false;
            }
        }

        public void DoAuthenticatedAndHandleRoles(Admin SignedAdmin)
        {
            boolIsAuthenticated = true;
            AdminId = SignedAdmin.Id;
            Firstname = SignedAdmin.Firstname;
            Lastname = SignedAdmin.Lastname;

            CommonFunctions.SetCookie("IranAtmp", string.Concat(CommonFunctions.EncryptWord(SignedAdmin.Id.ToString()), "-", CommonFunctions.EncryptWord(DateTime.Now.AddDays(7).ToString()), "-", CommonFunctions.EncryptWord(BrowserHelper.GetBrowserId())));


            var options = new DbContextOptionsBuilder<EFContext>().UseSqlServer(CommonFunctions.ConnectionString).Options;
            EFContext db = new EFContext(options);


            if (SignedAdmin.AdminRole?.Where(r => r.RecordDeleted == false).ToList().Count == 0)
            {
                return;
            }


            AdminRole? urDefault = SignedAdmin.AdminRole.Where(r => r.IsDefault == true && r.RecordDeleted == false).FirstOrDefault();

            if (urDefault is null)
            {
                urDefault = db.AdminRole.Where(r => r.RecordDeleted == false && r.AdminId == SignedAdmin.Id).OrderBy(u => u.Role).FirstOrDefault();


                urDefault.IsDefault = true;
                db.Entry(urDefault).State = EntityState.Modified;
                db.SaveChanges();

            }


            Role = urDefault.Role;


            RoleS = SignedAdmin.AdminRole;

            return;

        }

        public void DoLogout()
        {
            var context = HttpContextProvider.Current;

            var auth = context?.Request.Cookies["IranAtmp"];

            if (auth != null)
            {
                context?.Response.Cookies.Delete("IranAtmp");
                CommonFunctions.SetCookie("IranAtmp", string.Empty, -1);

            }

            boolIsAuthenticated = false;
            //Role = null;
            AdminId = 0;

        }

    }
}
