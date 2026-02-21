using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShafaHRCoreLib.Models
{
    public class LoginViewModel
    {
        [DisplayName("نام کاربری")]
        [MaxLength(50, ErrorMessage = "حداکثر تعداد کاراکتر 50 حرف است.")]
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        public string? UserName { get; set; }


        [DisplayName("کلمه عبور")]
        [MaxLength(50, ErrorMessage = "حداکثر تعداد کاراکتر 20 حرف است.")]
        [MinLength(4, ErrorMessage = "حداقل تعداد کاراکتر 4 حرف است.")]
        [Required(ErrorMessage = "کلمه عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
