using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShafaHRCoreLib.Models
{
    [Table("RecordBase")]
    public abstract class RecordBase
    {
        [MaxLength(128)]
        public string? RecordKey { get; set; }
        public DateTime? RecordCreated { get; set; }
        public DateTime? RecordModified { get; set; }
        public bool RecordDeleted { get; set; }

        public RecordChangeLog SetRecordDataForCreate(EFContext context, long? userId)
        {
            this.RecordKey = Guid.NewGuid().ToString();
            this.RecordCreated = DateTime.Now;
            this.RecordModified = DateTime.Now;
            this.RecordDeleted = false;

            RecordChangeLog log = RecordChangeLog.Create(this, RecordChangeAction.Create, userId);

            context.RecordChangeLog.Add(log);

            return log;
        }

        public RecordChangeLog SetRecordDataForModify(EFContext context, long? userId)
        {
            return SetRecordDataForModify(context, userId, "");
        }

        public RecordChangeLog SetRecordDataForModify(EFContext context, long? userId, string description)
        {
            this.RecordModified = DateTime.Now;

            RecordChangeLog log = RecordChangeLog.Create(this, RecordChangeAction.Modifiy, userId, description);

            context.RecordChangeLog.Add(log);

            return log;
        }

        public RecordChangeLog SetRecordDataForDelete(EFContext context, long? userId)
        {
            this.RecordDeleted = true;
            this.RecordModified = DateTime.Now;

            RecordChangeLog log = RecordChangeLog.Create(this, RecordChangeAction.Delete, userId);

            context.RecordChangeLog.Add(log);

            return log;
        }
    }
}
