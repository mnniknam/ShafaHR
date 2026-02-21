using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShafaHRCoreLib.Models
{
    public class ChangePasswordViewModel
    {
        [DisplayName("کلمه عبور قدیمی")]
        [MaxLength(20)]
        [Required(ErrorMessage = "کلمه عبور قبلی را وارد نمایید.")]
        public string? OldPassword { get; set; }

        [DisplayName("کلمه عبور جدید")]
        [MaxLength(20)]
        [Required(ErrorMessage = "کلمه عبور جدید را وارد نمایید.")]
        public string? NewPassword { get; set; }

        [DisplayName("تکرار کلمه عبور جدید")]
        [MaxLength(20)]
        [Required(ErrorMessage = "تکرار کلمه عبور جدید را وارد نمایید.")]
        public string? ConfirmPassword { get; set; }
    }
}
