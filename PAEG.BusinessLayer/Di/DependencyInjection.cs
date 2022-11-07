using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.BusinessLayer.Voter;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Di;

public static class DependencyInjection {
    public static IServiceCollection AddBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddSingleton<IDecryptionService, DecryptionService>(x =>
            new DecryptionService(x.GetService<IVoterProvider>()!,
                x.GetService<IVoterRandomStringsProvider>()!,
                int.Parse(configuration["UserAmount"])
            )
        );
        services.AddSingleton<IVotesCalculationService, VotesCalculationService>(x =>
            new VotesCalculationService(
                x.GetService<IVoterProvider>()!,
                x.GetService<IVoterRandomStringsProvider>()!,
                int.Parse(configuration["UserAmount"])
            )
        );

        return services;
    }
}