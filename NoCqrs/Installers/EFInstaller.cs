using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoCqrs.DataAccess;
using NoCqrs.Domain;

namespace NoCqrs.Installers
{
    public static class EFInstaller
    {
        public static IServiceCollection AddEF(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InsuranceDbContext>(opts =>
            {
                opts.UseInMemoryDatabase("InsuranceDb");
            });
            services.AddScoped<IDataStore, EFDataStore>();
            return services;
        }
    }
}