using ShafaHRCoreLib.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShafaHRCoreLib.Models
{
    public enum RecordChangeAction { None, Create, Modifiy, Delete, Others }

    [Table("RecordChangeLog")]

    public class RecordChangeLog
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(128)]
        public string? RecordType { get; set; }

        [MaxLength(128)]
        public string? RecordKey { get; set; }

        public DateTime DateOf { get; set; }

        public long? AdminId { get; set; }

        public string? Description { get; set; }

        public RecordChangeAction Action { get; set; }

        public RecordChangeAction RecordChangeAction
        {
            get => default(RecordChangeAction);
            set
            {
            }
        }

        public static RecordChangeLog Create(RecordBase record, RecordChangeAction action, long? userId)
        {
            return Create(record, action, userId, "");
        }

        public static RecordChangeLog Create(RecordBase record, RecordChangeAction action, long? adminId, string description)
        {
            RecordChangeLog log = new RecordChangeLog();

            log.RecordType = CommonFunctions.GetUnproxiedType(record).Name;

            log.RecordKey = record.RecordKey;

            log.DateOf = record.RecordModified.Value;
            log.Action = action;
            log.AdminId = adminId;
            log.Description = description;

            return log;
        }
    }
}
