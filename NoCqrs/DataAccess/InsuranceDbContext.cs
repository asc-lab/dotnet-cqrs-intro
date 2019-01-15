using Microsoft.EntityFrameworkCore;
using NoCqrs.Domain;

namespace NoCqrs.DataAccess
{
    public class InsuranceDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Policy> Policies { get; set; }

        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(product => product.Id);
                var partsNav = p.Metadata.FindNavigation(nameof(Product.Covers));
                partsNav.SetPropertyAccessMode(PropertyAccessMode.Field);    
            });
            
            modelBuilder.Entity<Offer>(o =>
            {
                o.HasKey(offer => offer.Id);
                var partsNav = o.Metadata.FindNavigation(nameof(Offer.Covers));
                partsNav.SetPropertyAccessMode(PropertyAccessMode.Field);

                o.OwnsOne(offer => offer.Customer);
                o.OwnsOne(offer => offer.Driver);
                o.OwnsOne(offer => offer.Car);

            });
            
            modelBuilder.Entity<Policy>(p =>
            {
                p.HasKey(policy => policy.Id);
                var partsNav = p.Metadata.FindNavigation(nameof(Policy.Versions));
                partsNav.SetPropertyAccessMode(PropertyAccessMode.Field);
            });
            
            modelBuilder.Entity<PolicyVersion>(pv =>
            {
                pv.HasKey(policyVersion => policyVersion.Id);
                var partsNav = pv.Metadata.FindNavigation(nameof(PolicyVersion.Covers));
                partsNav.SetPropertyAccessMode(PropertyAccessMode.Field);

                pv.OwnsOne(policyVersion => policyVersion.PolicyHolder);
                pv.OwnsOne(policyVersion => policyVersion.Driver);
                pv.OwnsOne(policyVersion => policyVersion.Car);

                pv.OwnsOne(policyVersion => policyVersion.CoverPeriod);
                pv.OwnsOne(policyVersion => policyVersion.VersionValidityPeriod);
            });
            
            modelBuilder.Entity<PolicyCover>(pc =>
            {
                pc.HasKey(c => c.Id);
                pc.OwnsOne(c => c.CoverPeriod);
            });
            
        }
    }
}