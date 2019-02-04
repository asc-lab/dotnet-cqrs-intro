using Microsoft.Extensions.DependencyInjection;

namespace SeparateModels.ReadModels
{
    public static class ReadModelsInstaller
    {
        public static IServiceCollection AddReadModels(this IServiceCollection services, string cnnString)
        {
            services
                .AddPolicyInfoDtoProjection(cnnString)
                .AddPolicyInfoDtoFinder(cnnString);
            
            return services;
        }
        
        private static IServiceCollection AddPolicyInfoDtoProjection(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(new PolicyInfoDtoProjection(cnnString));
            services.AddSingleton(new PolicyVersionDtoProjection(cnnString));
            return services;
        }
        
        private static IServiceCollection AddPolicyInfoDtoFinder(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton(new PolicyInfoDtoFinder(cnnString));
            return services;
        }
    }
}