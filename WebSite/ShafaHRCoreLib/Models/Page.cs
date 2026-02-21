using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShafaHRCoreLib.Models
{
    public enum EnumPageCode
    {

        [Display(Name = "درباره ما")]
        About = 1,

        [Display(Name = "تیم علمی")]
        ScientificTeam = 2,

        [Display(Name = "سازمان های طرف قرارداد")]
        Organizations = 3,
    }

    [Table("Page")]
    public class Page : RecordBase
    {

        [Key]
        public long Id { get; set; }


        [DisplayName("عنوان")]
        [MaxLength(128, ErrorMessage = "حداکثر تعداد کاراکتر 128 حرف است.")]
        [Required(ErrorMessage = "نام را وارد نمایید")]
        public string? Title { get; set; }


        [DisplayName("کد")]
        public EnumPageCode? Code { get; set; }


        [DisplayName("محتوای متنی")]
        [MaxLength(10000, ErrorMessage = "حداکثر تعداد کاراکتر 10000 حرف است.")]
        public string? BodyText { get; set; }


        [DisplayName("محتوای وب")]
        [MaxLength(10000, ErrorMessage = "حداکثر تعداد کاراکتر 10000 حرف است.")]
        public string? BodyHTML { get; set; }
    }
}
