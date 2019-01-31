using System;
using System.Threading.Tasks;
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

        public async Task Seed()
        {
            var product = await dataStore.Products.WithCode("STD_CAR_INSURANCE");
            if (product == null)
            {
                product = Products.StandardCarInsurance();
                dataStore.Products.Add(product);
                await dataStore.CommitChanges();
            }

            
            var offer = await dataStore.Offers.WithNumber("OFF-001");
            if (offer == null)
            {
                dataStore.Offers.Add(
                    Offers.StandardOneYearOCOfferValidUntil(product, "OFF-001", SysTime.CurrentTime.AddDays(15)));
            }
        
            
            var offer2 = await dataStore.Offers.WithNumber("OFF-002");
            if (offer2 == null)
            {
                dataStore.Offers.Add(
                    Offers.StandardOneYearOCOfferValidUntil(product, "OFF-002", SysTime.CurrentTime.AddDays(-3)));
            }

            await dataStore.CommitChanges();
            
        }
    }
    
    public static class ApplicationBuilderExtensions
    {
        public static async Task UseDbInitializer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var dataStore = scope.ServiceProvider.GetService<IDataStore>();
                await new DbInitializer(dataStore).Seed();
            }
        }
    }
}