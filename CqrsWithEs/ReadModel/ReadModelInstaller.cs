using CqrsWithEs.Init;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsWithEs.ReadModel
{
    public static class ReadModelInstaller
    {
        public static IServiceCollection AddReadModels(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(new PolicyInfoDao(cnnString));
            return services;
        }
    }
}