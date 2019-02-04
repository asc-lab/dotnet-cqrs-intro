using Microsoft.Extensions.DependencyInjection;

namespace SeparateModels.ReadModels
{
    public static class ReadModelsInstaller
    {
        public static IServiceCollection AddReadModels(this IServiceCollection services, string cnnString)
        {
            services
                .AddProjections(cnnString)
                .AddFinders(cnnString);
            
            return services;
        }
        
        private static IServiceCollection AddProjections(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(new PolicyInfoDtoProjection(cnnString));
            services.AddSingleton(new PolicyVersionDtoProjection(cnnString));
            return services;
        }
        
        private static IServiceCollection AddFinders(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(new PolicyInfoDtoFinder(cnnString));
            services.AddSingleton(new PolicyVersionDtoFinder(cnnString));
            return services;
        }
    }
}