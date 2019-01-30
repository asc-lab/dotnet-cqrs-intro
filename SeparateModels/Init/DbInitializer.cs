using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SeparateModels.Domain;

namespace SeparateModels.Init
{
    public class DbInitializer
    {
        private readonly IDataStore dataStore;

        public DbInitializer(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public void Seed()
        {
            dataStore.Products.Add(Products.StandardCarInsurance());
            dataStore.CommitChanges();

            var product = dataStore.Products.WithCode("STD_CAR_INSURANCE");

            dataStore.Offers.Add(Offers.StandardOneYearOCOfferValidUntil(product, "OFF-001", SysTime.CurrentTime.AddDays(15)));
            dataStore.Offers.Add(Offers.StandardOneYearOCOfferValidUntil(product, "OFF-002", SysTime.CurrentTime.AddDays(-3)));
            dataStore.CommitChanges();
            
        }
    }
    
    public static class ApplicationBuilderExtensions
    {
        public static void UseDbInitializer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dataStore = scope.ServiceProvider.GetService<IDataStore>();
                new DbInitializer(dataStore).Seed();
            }
        }
    }
}