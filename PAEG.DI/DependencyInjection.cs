using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.BusinessLayer.Di;
using PAEG.PersistenceLayer.Di;

namespace PAEG.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddApiDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddPersistenceLayer(configuration);
        services.AddBusinessLayer(configuration);

        return services;
    }
}