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
        var userdataProvider = new InMemoryUserProvider(
            int.Parse(configuration["UserAmount"]),
            configuration["Candidates"].Length
        );
        services.AddSingleton<IUserProvider>(_ => userdataProvider);
        services.AddSingleton<ITableProvider, InMemoryTableProvider>();
        services.AddSingleton<IVotingCentreProvider, InMemoryVotingProvider>();
        services.AddSingleton<IRegistrationProvider, RegistrationProvider>();

        return services;
    }
}