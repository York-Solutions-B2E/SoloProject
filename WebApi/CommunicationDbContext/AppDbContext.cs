
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.CommunicationDbContext
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Communication> Communications { get; set; }
        public DbSet<CommunicationStatusHistory> CommunicationStatusHistories { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }
        public DbSet<CommunicationTypeStatus> CommunicationTypeStatuses { get; set; }

        //Explicit is safer than implicity for EFCore
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Communication>()
            .HasKey(c => c.Id);

            modelBuilder.Entity<CommunicationStatusHistory>()
                .HasOne(h => h.Communication)
                .WithMany(c => c.StatusHistory)
                .HasForeignKey(h => h.CommunicationId);

            modelBuilder.Entity<CommunicationType>()
                .HasKey(t => t.TypeCode);
            //Composite key
            modelBuilder.Entity<CommunicationTypeStatus>()
                .HasKey(ts => new { ts.TypeCode, ts.StatusCode });

            modelBuilder.Entity<CommunicationTypeStatus>()
                .HasOne(ts => ts.CommunicationType)
                .WithMany(t => t.Statuses)
                .HasForeignKey(ts => ts.TypeCode);
                modelBuilder.Entity<CommunicationType>().HasData(
                    new CommunicationType { TypeCode = "EOB", DisplayName = "Explanation of Benefits" },
                    new CommunicationType { TypeCode = "IDCARD", DisplayName = "ID Card" }
            );

        }
        
    }

}