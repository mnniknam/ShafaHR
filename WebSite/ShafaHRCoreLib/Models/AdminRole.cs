using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShafaHRCoreLib.Models
{
    public enum EnumAdminRole
    {
        [Display(Name = "ادمین")]
        SysAdmin = 1,

        [Display(Name = "اپراتور صفحات")]
        EditorPages = 2,

        [Display(Name = "اپراتور انتشارات")]
        EditorPublications = 3,
    }

    [Table("AdminRole")]
    public class AdminRole : RecordBase
    {
        [Key]
        public long Id { get; set; }


        [DisplayName("نقش")]
        [Required(ErrorMessage = "نقش را انتخاب کنید.")]
        public EnumAdminRole Role { get; set; }


        public Boolean IsDefault { get; set; }


        [ForeignKey("Admin")]
        public long AdminId { get; set; }
        public virtual Admin? Admin { get; set; }

    }
}
