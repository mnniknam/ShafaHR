using Microsoft.EntityFrameworkCore;

namespace ShafaHRCoreLib.Models
{
    public class EFContext : DbContext
    {
        //public EFContext() : base("DBConnection")
        //public EFContext() : base(@"Data Source=DESKTOP-VA6LPPF\MSSQLSERVER2019;Initial Catalog=Sleep-Tums;Integrated Security=True;MultipleActiveResultSets=true")
        //public EFContext() : base(@"Data Source=.;Initial Catalog=MRSleep;user=sa;password=09353092852;MultipleActiveResultSets=true")

        public EFContext(DbContextOptions<EFContext> options)
           : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Centers>()
        //.Property("Discriminator")
        //.HasMaxLength(50); // یا بیشتر

        }

        public IQueryable<T> NotDeleted<T>() where T : RecordBase
        {
            return this.Set<T>().Where(r => r.RecordDeleted != true);
        }

        public DbSet<RecordChangeLog> RecordChangeLog { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<AdminRole> AdminRole { get; set; }

        //public DbSet<User> User { get; set; }

        public DbSet<File> File { get; set; }

        
        public DbSet<Publication> Publication { get; set; }

      
        public DbSet<Page> Page { get; set; }

    }
}
