using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShafaHRCoreLib.Managers
{
    public static class CommonFunctions
    {
        //public static string FileFolderPath = "C:\\Project\\CentersRegistration\\Files";
        //public static string FileFolderPath = "C:\\Project\\DrFarshidAlaeddini\\Files";
        public static string FileFolderPath = "C:\\Websites\\IranAtmp\\Files";

        //public static string ConnectionString = "Data Source =.; Initial Catalog = IranAtmp; user=sa;password=09353092852;MultipleActiveResultSets=true;TrustServerCertificate=True";
        //public static string ConnectionString = "Data Source = DESKTOP-VA6LPPF\\MSSQLSERVER2019; Initial Catalog = IranAtmp; Integrated Security=True;Encrypt=False;";
        public static string ConnectionString = "Data Source =.; Initial Catalog = IranAtmp; Integrated Security=True;MultipleActiveResultSets=true;TrustServerCertificate=True";


        public static string GetSHA1(string sText)
        {
            if (sText == null || sText == "")
            {
                return "";
            }

            System.Security.Cryptography.SHA1CryptoServiceProvider sHA1Crypto = new System.Security.Cryptography.SHA1CryptoServiceProvider();

            byte[] raw = System.Text.Encoding.UTF8.GetBytes(sText);

            byte[] res = sHA1Crypto.ComputeHash(raw);

            return System.Convert.ToBase64String(res);


        }

        public static bool CheckNationalCode(string nationalCode)
        {
            try
            {

                //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                if (allDigitEqual.Contains(nationalCode)) return false;


                //عملیات شرح داده شده در بالا
                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());

                var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                var c = b % 11;

                return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
            }
            catch { return false; }

        }

        public static int GetYears(DateTime start, DateTime end)
        {
            return (end.Year - start.Year - 1) +
                (((end.Month > start.Month) ||
                ((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
        }

        public static string GetPersianDate(DateTime enDate)
        {
            if (enDate == null)
                return "";

            System.Globalization.PersianCalendar shamsi = new System.Globalization.PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(shamsi.GetYear(enDate));
            sb.Append("/");

            if (shamsi.GetMonth(enDate) < 10)
            {
                sb.Append("0");
            }

            sb.Append(shamsi.GetMonth(enDate));
            sb.Append("/");

            if (shamsi.GetDayOfMonth(enDate) < 10)
            {
                sb.Append("0");
            }

            sb.Append(shamsi.GetDayOfMonth(enDate));

            return sb.ToString();

            // return (string.Format("{0}/{1}/{2}", shamsi.GetYear(enDate), shamsi.GetMonth(enDate), shamsi.GetDayOfMonth(enDate)));
        }

        public static string GetDisplayName(this Enum val)
        {
            if (val == null) { return "----"; }
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }

        public static bool IsValidNationalCode(string nationalCode)
        {
            try
            {

                //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
                var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
                if (allDigitEqual.Contains(nationalCode)) return false;


                //عملیات شرح داده شده در بالا
                var chArray = nationalCode.ToCharArray();
                var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
                var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
                var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
                var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
                var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
                var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
                var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
                var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
                var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
                var a = Convert.ToInt32(chArray[9].ToString());

                var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
                var c = b % 11;

                return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
            }
            catch { return false; }
        }

        public static string RevestString(string text)
        {
            char[] textToChar = text.ToCharArray();
            string result = string.Empty;
            int length = textToChar.Length;
            for (int i = length; i > 0; --i)
                result += textToChar[i - 1];
            return result;
        }

        public static string RevestDateString(string dateText)
        {
            string[] arrT = dateText.Split(new Char[] { '/' });

            return arrT[2] + "/" + arrT[1] + "/" + arrT[0];
        }

        public static DateTime GetEnDate(string dateTextShamsi)
        {

            string[] arrT = dateTextShamsi.Split(new Char[] { '/' });
            System.Globalization.PersianCalendar gb = new System.Globalization.PersianCalendar();

            return gb.ToDateTime(int.Parse(arrT[2]), int.Parse(arrT[0]), int.Parse(arrT[1]), 0, 0, 0, 0);


        }

        public static DateTime GetEnDateB(string dateTextShamsi)
        {

            string[] arrT = dateTextShamsi.Split(new Char[] { '/' });
            System.Globalization.PersianCalendar gb = new System.Globalization.PersianCalendar();

            return gb.ToDateTime(int.Parse(arrT[0]), int.Parse(arrT[1]), int.Parse(arrT[2]), 0, 0, 0, 0);


        }

        public static string GetDateDirectoryName(DateTime dateTime)
        {

            StringBuilder strBld = new StringBuilder();
            strBld.Append(dateTime.Year.ToString().Substring(2, 2));

            if (dateTime.Month < 10)
            { strBld.Append("0"); }


            strBld.Append(dateTime.Month);

            return strBld.ToString();

        }

        public static string GetEnglishNumber(string Text)
        {
            return Text.Trim().Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
        }

        public static string EncryptWord(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("12345678912345678912123456789521");
                aesAlg.IV = new byte[16];
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptWord(string cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;

            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("12345678912345678912123456789521");
                aesAlg.IV = new byte[16];
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        public static string ConvertArabicToFarsi(string cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            return cipherText.Replace("ي", "ی").Replace("ك", "ک");
        }

        public static long? ConvertStringMinute(string TimeText)
        {
            if (string.IsNullOrEmpty(TimeText))
                return null;

            string[] arr = TimeText.Split(':');

            if (arr.Length != 2)
                return null;

            if (int.TryParse(arr[0], out int Hour) || int.TryParse(arr[1], out int Minute)) { return null; }

            return Hour * 60 + Minute;

        }

        public static Type GetUnproxiedType(object entity)
        {
            var type = entity.GetType();
            return type.Namespace == "Castle.Proxies" && type.BaseType != null
                ? type.BaseType
                : type;
        }

        //public static bool SetCookie(string cookieName, string cookieValue, int expireDays = 7)
        //{
        //    var context = HttpContextProvider.Current;

        //    if (context != null)
        //    {
        //        context.Response.Cookies.Append(
        //            cookieName,
        //            cookieValue,
        //            new CookieOptions
        //            {
        //                HttpOnly = true,       // فقط از سرور قابل دسترسی است
        //                Secure = true,         // فقط روی HTTPS ارسال شود
        //                Expires = DateTimeOffset.UtcNow.AddDays(expireDays)
        //            }
        //        );
        //    }
        //    return true;
        //}

        //public static bool DeleteCookie(string cookieName)
        //{
        //    var context = HttpContextProvider.Current;

        //    if (context != null)
        //    {
        //        context.Response.Cookies.Delete(cookieName);
        //    }
        //    return true;
        //}
    }
}
