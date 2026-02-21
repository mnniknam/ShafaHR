using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShafaHRCoreLib.Models
{
    [Table("Admin")]
    public class Admin : RecordBase
    {
        [Key]
        public long Id { get; set; }


        [DisplayName("نام")]
        [MaxLength(128, ErrorMessage = "حداکثر تعداد کاراکتر 128 حرف است.")]
        [Required(ErrorMessage = "نام را وارد نمایید")]
        public string? Firstname { get; set; }


        [DisplayName("نام خانوادگی")]
        [MaxLength(128, ErrorMessage = "حداکثر تعداد کاراکتر 128 حرف است.")]
        [Required(ErrorMessage = "نام خانوادگی را وارد نمایید")]
        public string? Lastname { get; set; }


        [DisplayName("شماره همراه:")]
        [MinLength(11, ErrorMessage = "تعداد کاراکتر 11 حرف است.")]
        [MaxLength(20, ErrorMessage = "تعداد کاراکتر 20 حرف است.")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "شماره همراه صحیح نیست.")]
        public string? Mobile { get; set; }


        [DisplayName("نام کاربری")]
        [MaxLength(50, ErrorMessage = "حداکثر تعداد کاراکتر 50 حرف است.")]
        [Required(ErrorMessage = "نام کاربری را وارد نمایید")]
        public string? UserName { get; set; }


        [DisplayName("کلمه عبور")]
        [MaxLength(50, ErrorMessage = "حداکثر تعداد کاراکتر 20 حرف است.")]
        [MinLength(4, ErrorMessage = "حداقل تعداد کاراکتر 4 حرف است.")]
        [Required(ErrorMessage = "کلمه عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; }


        [InverseProperty("Admin")]
        public virtual ICollection<AdminRole>? AdminRole { get; set; } = new List<AdminRole>();



    }
}
}
