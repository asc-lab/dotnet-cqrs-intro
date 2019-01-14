using Microsoft.EntityFrameworkCore;
using NoCqrs.Domain;

namespace NoCqrs.DataAccess
{
    public class InsuranceDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Offer> Offers { get; set; }

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
            
        }
    }
}