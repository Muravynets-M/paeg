using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.Di;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        var userdataProvider = new InMemoryVoterProvider(
            int.Parse(configuration["UserAmount"])
        );
        services.AddSingleton<IVoterProvider>(_ => userdataProvider);
        services.AddSingleton<ICandidateProvider, InMemoryCandidateProvider>();
        services.AddSingleton<IEcProvider, InMemoryEcDataProvide>();
        services.AddSingleton<ICecProvider, InMemoryCecProvider>();

        return services;
    }
}