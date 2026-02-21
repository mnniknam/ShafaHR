using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShafaHRCoreLib.Models
{
    [Table("File")]
    public class File : RecordBase
    {
        [Key]
        public long Id { get; set; }


        [StringLength(100)]
        public string? FileName { get; set; }


        [StringLength(200)]
        public string? ContentType { get; set; }


        [StringLength(20)]
        public string? Extension { get; set; }


    }
}
