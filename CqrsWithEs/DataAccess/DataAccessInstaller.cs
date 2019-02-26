using CqrsWithEs.Domain.Offer;
using CqrsWithEs.Domain.Policy;
using CqrsWithEs.Domain.Product;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsWithEs.DataAccess
{
    public static class DataAccessInstaller
    {
        public static void AddDataAccess(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, InMemoryProductsRepository>();
            services.AddSingleton<IOfferRepository, InMemoryOfferRepository>();
            services.AddScoped<IPolicyRepository, InMemoryPolicyRepository>();
            services.AddSingleton<IEventStore, EventStore>();
        }
    }
}