using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core.Models.States;
using vegaplanner.Core.Models;
using vegaplanner.Core.Models.Security;

namespace vega.Persistence
{
    public class VegaDbContext : IdentityDbContext<AppUser>
    {
        
        public DbSet<Make> Makes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<StateInitialiser> StateInitialisers { get; set; }
        public DbSet<StateInitialiserState> StateInitialiserState { get; set; }
        public DbSet<PlanningApp> PlanningApps { get; set; }
        public DbSet<PlanningAppState> PlanningAppState { get; set; }
        public DbSet<Drawing> Drawings { get; set; }
        public DbSet<StateStatus> StateStatus { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> CustomerAddress { get; set; }
        public DbSet<Address> DevelopmentAddress { get; set; }
        public DbSet<StateInitialiserCustomField> StateInitialiserCustomFields { get; set; }
        public DbSet<DevelopmentType> DevelopmentType { get; set; }

        //Security based contexts
        public DbSet<InternalAppUser> AppUsers { get; set; }

        public VegaDbContext(DbContextOptions<VegaDbContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
                base.OnModelCreating(modelBuilder);    //Required for Identity User!!!!

                modelBuilder.Entity<VehicleFeature>().HasKey(vf => new { vf.VehicleId, vf.FeatureId });
                modelBuilder.Entity<Vehicle>().OwnsOne(c => c.Contact);

                modelBuilder.Entity<PlanningApp>().OwnsOne(c => c.Developer);
                modelBuilder.Entity<PlanningApp>().OwnsOne(c => c.DevelopmentAddress);

                modelBuilder.Entity<Customer>().OwnsOne(c => c.CustomerAddress);
                modelBuilder.Entity<Customer>().OwnsOne(c => c.CustomerContact);
                
                modelBuilder.Entity<StateInitialiserStateCustomField>().HasKey(sr => new { sr.StateInitialiserStateId, sr.StateInitialiserCustomFieldId });

                // modelBuilder.Entity<PlanningAppState>()
                //     .OwnsOne(b => b.PlanningApp)
                //     .OnDelete(DeleteBehavior.Restrict);       

        }
    }
}