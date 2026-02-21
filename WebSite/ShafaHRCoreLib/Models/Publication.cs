using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShafaHRCoreLib.Models
{
    public enum EnumPublicationStatus
    {
        [Display(Name = "پیش نویس")]
        Draft = 1,

        [Display(Name = "انتشار یافته")]
        Published = 2,

        [Display(Name = "تایید نشده")]
        NotConfirmed = 3,

    }

    public enum EnumPublicationCategory
    {
        [Display(Name = "متون علمی")]
        ScientificTexts = 1,

        [Display(Name = "کتب")]
        Book = 2,

        [Display(Name = "خبرنامه")]
        Newsletter = 3,

        [Display(Name = "گزیده خلاصه مقالات اپیدمیولوژی و آمار")]
        Papers = 4,
    }

    [Table("Publication")]
    public class Publication : RecordBase
    {
        public Publication()
        {
            Status = EnumPublicationStatus.Draft;
        }

        [Key]
        public long Id { get; set; }


        [DisplayName("عنوان انتشارات")]
        [MaxLength(1000, ErrorMessage = "حداکثر تعداد کاراکتر 1000 حرف است.")]
        [Required(ErrorMessage = "عنوان خبر را وارد نمایید")]
        public string? Title { get; set; }


        [DisplayName("وضعیت")]
        public EnumPublicationStatus Status { get; set; }


        [DisplayName("دسته بندی")]
        public EnumPublicationCategory Category { get; set; }


        [DisplayName("خلاصه")]
        [MaxLength(3000, ErrorMessage = "حداکثر تعداد کاراکتر 3000 حرف است.")]
        [Required(ErrorMessage = "خلاصه انتشارات را وارد نمایید")]
        [DataType(DataType.MultilineText)]
        public string? Summary { get; set; }


        [DisplayName("متن")]
        [MaxLength(10000, ErrorMessage = "حداکثر تعداد کاراکتر 10000 حرف است.")]
        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }


        [DisplayName("فایل تصویر")]
        [ForeignKey("Thumbnail")]
        public long? ThumbnailId { get; set; }
        public virtual File? Thumbnail { get; set; }


        [DisplayName("فایل پی دی اف")]
        [ForeignKey("PDF")]
        public long? PDFId { get; set; }
        public virtual File? PDF { get; set; }


        [DisplayName("فایل ویدئو")]
        [ForeignKey("VIDEO")]
        public long? VideoId { get; set; }
        public virtual File? Video { get; set; }


        [DisplayName("تعداد بازدید")]
        public long ViewCount { get; set; }
    }
}
