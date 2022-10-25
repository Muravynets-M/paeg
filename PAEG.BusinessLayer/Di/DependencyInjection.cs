using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PAEG.BusinessLayer.Chains.Decoding;
using PAEG.BusinessLayer.Chains.Encoding;
using PAEG.BusinessLayer.Services.Calculation;
using PAEG.BusinessLayer.Services.RegistrationService;
using PAEG.BusinessLayer.Services.Voting;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Di;

public static class DependencyInjection {
    public static IServiceCollection AddBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IVotingService, VotingService>();
        services.AddSingleton<IEncodingChain, ElGamalEncoder>(
            x => new ElGamalEncoder(
                new DsaSignEncoder(
                    new TerminalEncoder(
                        x.GetService<IVotingCentreProvider>()!
                    ),
                    x.GetService<ITableProvider>()!
                ),
                x.GetService<ITableProvider>()!
            ));
        services.AddSingleton<ICalculationService, CalculationService>();
        services.AddSingleton<IDecodingChain, BallotDecoder>(
            x => new BallotDecoder(
                new RsaSignDecoder(
                    new ElGamalDecoder(
                        new VoteDecoder(
                            x.GetService<ITableProvider>()!,
                            x.GetService<IVotingCentreProvider>()!,
                            int.Parse(configuration["Candidates"])
                        ),
                        x.GetService<ITableProvider>()!
                    ),
                    x.GetService<ITableProvider>()!
                ),
                x.GetService<IVotingCentreProvider>()!,
                x.GetService<IRegistrationProvider>()!
            )
        );
        services.AddSingleton<IRegistrationService, RegistrationService>();

        return services;
    }
}