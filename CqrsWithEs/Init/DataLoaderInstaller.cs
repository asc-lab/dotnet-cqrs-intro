using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CqrsWithEs.Init
{
    public static class DataLoaderInstaller
    {
        public static IServiceCollection AddDataInitializer(this IServiceCollection services)
        {
            services.AddScoped<DataLoader>();
            return services;
        }
    }
    
    public static class ApplicationBuilderExtensions
    {
        public static async Task UseDataInitializer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetService<DataLoader>();
                await initializer.Seed();
            }
        }
    }
}