using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.BusinessLayer.Services.Calculation;
using PAEG.BusinessLayer.Services.Voting;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Di;

public static class DependencyInjection {
    public static IServiceCollection AddBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IVotingService>(x => new VotingService(
            int.Parse(configuration["Candidates"]), 
            x.GetService<IVotingCentreDataProvider>()!,
            x.GetService<ITableProvider>()!
        ));
        services.AddSingleton<ICalculationService, CalculationService>();

        return services;
    }
}