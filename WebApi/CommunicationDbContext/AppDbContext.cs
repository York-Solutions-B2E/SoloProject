
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
        
        public DbSet<GlobalStatuses> GlobalStatuses { get; set; }

        //Explicit is safer than implicity for EFCore
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GlobalStatuses>()
            .HasKey(gs => gs.StatusCode);

            modelBuilder.Entity<GlobalStatuses>().HasData(
            // Creation phase
            new GlobalStatuses { StatusCode = "ReadyForRelease", Description = "Ready For Release" },
            new GlobalStatuses { StatusCode = "Released", Description = "Released" },

            // Production phase
            new GlobalStatuses { StatusCode = "QueuedForPrinting", Description = "Queued For Printing" },
            new GlobalStatuses { StatusCode = "Printed", Description = "Printed" },
            new GlobalStatuses { StatusCode = "Inserted", Description = "Inserted" },
            new GlobalStatuses { StatusCode = "WarehouseReady", Description = "Warehouse Ready" },

            // Logistics phase
            new GlobalStatuses { StatusCode = "Shipped", Description = "Shipped" },
            new GlobalStatuses { StatusCode = "InTransit", Description = "In Transit" },
            new GlobalStatuses { StatusCode = "Delivered", Description = "Delivered" },
            new GlobalStatuses { StatusCode = "Returned", Description = "Returned" },

            // Additional statuses
            new GlobalStatuses { StatusCode = "Failed", Description = "Failed" },
            new GlobalStatuses { StatusCode = "Cancelled", Description = "Cancelled" },
            new GlobalStatuses { StatusCode = "Expired", Description = "Expired" },
            new GlobalStatuses { StatusCode = "Archived", Description = "Archived" }
            );

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