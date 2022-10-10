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
        services.AddSingleton<IUserDataProvider>(x => new InMemoryUserDataProvider(
            int.Parse(configuration["UserAmount"]),
            configuration["Candidates"].Length,
            int.Parse(configuration["PackSize"])
        ));
        services.AddSingleton<ITableProvider, InMemoryTableProvider>();
        services.AddSingleton<IVotingCentreDataProvider, InMemoryVotingDataProvider>();

        return services;
    }
}