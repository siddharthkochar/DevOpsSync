using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevOpsSync.WebApp.API.Data
{
    public class DevOpsSyncDbContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Trigger> Triggers { get; set; }
        public DbSet<ServiceAction> Actions { get; set; }

        public DbSet<ServiceTriggerAction> ServiceTriggerAction { get; set; }

        public DevOpsSyncDbContext(DbContextOptions<DevOpsSyncDbContext> options)
            : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Service>();

            modelBuilder.Entity<Service>()
                .HasData(new Service[]
                {
                    new Service()
                    {
                        Id = 1,
                        Name = "GitHub",
                        ImageUrl = "https://assets.ifttt.com/images/channels/2107379463/icons/monochrome_large.png",
                        Color = "#00aeef"
                    },
                    new Service()
                    {
                        Id = 2,
                        Name = "Visual Studio Team Service",
                        ImageUrl = "",
                        Color = "#0da778"
                    }
                });


            modelBuilder.Entity<Trigger>(entity =>
            {
                entity.HasOne(x => x.Service)
                    .WithMany(x => x.Triggers)
                    .HasForeignKey(x => x.ServiceId);
            });

            modelBuilder.Entity<ServiceAction>(entity =>
            {
                entity.HasOne(x => x.Service)
                    .WithMany(x => x.Actions)
                    .HasForeignKey(x => x.ServiceId);
            });

            modelBuilder.Entity<Trigger>()
                .HasData(new Trigger[]
                {
                    new Trigger()
                    {
                        Id = 1,
                        Name = "Push",
                        Description = "When something is pushed on",
                        ServiceId = 1
                    }
                });

            modelBuilder.Entity<ServiceAction>()
                .HasData(new ServiceAction[]
                {
                    new ServiceAction()
                    {
                        Id = 1,
                        Name = "Create Issue",
                        ServiceId = 1
                    },
                    new ServiceAction()
                    {
                        Id = 2,
                        Name = "Sample Action 2",
                        ServiceId = 1
                    }
                });


            modelBuilder.Entity<ServiceTriggerAction>(entity =>
            {
                entity.HasKey(t => new {t.TriggerId, t.ActionId});
            });

            modelBuilder.Entity<ServiceTriggerAction>()
                .HasOne(sta => sta.Trigger)
                .WithMany(x => x.ServiceTriggerActions)
                .HasForeignKey(sta => sta.TriggerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ServiceTriggerAction>()
                .HasOne(sta => sta.ServiceAction)
                .WithMany(x => x.ServiceTriggerActions)
                .HasForeignKey(sta => sta.ActionId)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<ServiceTriggerAction>()
                .HasData(new ServiceTriggerAction[]
                {
                    new ServiceTriggerAction()
                    {
                        TriggerId = 1,
                        ActionId = 1
                    }
                });
        }
    }

    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }

        public virtual ICollection<Trigger> Triggers { get; set; }
        public virtual ICollection<ServiceAction> Actions { get; set; }
    }

    public class Trigger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }


        public virtual ICollection<ServiceTriggerAction> ServiceTriggerActions { get; set; }
    }

    public class ServiceAction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }

        public virtual ICollection<ServiceTriggerAction> ServiceTriggerActions { get; set; }
    }

    public class ServiceTriggerAction
    {
        public int TriggerId { get; set; }
        public virtual Trigger Trigger { get; set; }

        public int ActionId { get; set; }
        public virtual ServiceAction ServiceAction { get; set; }
    }
}
