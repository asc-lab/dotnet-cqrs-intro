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
            services.AddSingleton<IPolicyRepository, InMemoryPolicyRepository>();
            services.AddSingleton<IEventStore, EventStore>();
        }
    }
}