using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PAEG.BusinessLayer.Chains.Decoding;
using PAEG.BusinessLayer.Chains.Encoding;
using PAEG.BusinessLayer.Services.Calculation;
using PAEG.BusinessLayer.Services.Voting;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Di;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IVotingService, VotingService>();
        services.AddSingleton<IEncodingChain, GammaEncoder>(
            x => new GammaEncoder(
                new RsaSignEncoder(
                    new RsaEncodeEncoder(
                        new TerminalEncoder(
                            x.GetService<IVotingCentreDataProvider>()!
                        ),
                        x.GetService<ITableProvider>()!,
                        x.GetService<IVotingCentreDataProvider>()!
                    ),
                    x.GetService<ITableProvider>()!
                ),
                x.GetService<ITableProvider>()!)
        );
        services.AddSingleton<ICalculationService, CalculationService>();
        services.AddSingleton<IDecodingChain, RsaSignDecoder>(
            x => new RsaSignDecoder(
                new RsaDecodeDecoder(
                    new GammaDecoder(
                        new VoteDecoder(
                            x.GetService<ITableProvider>()!, 
                            x.GetService<IVotingCentreDataProvider>()!,
                            int.Parse(configuration["Candidates"])
                            ),
                        x.GetService<ITableProvider>()!,
                        x.GetService<ILogger<GammaDecoder>>()!
                        ),
                    x.GetService<ITableProvider>()!
                    ),
                x.GetService<ITableProvider>()!
            )
        );

        return services;
    }
}