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
        services.AddSingleton<IEcSendingService, EcSendingService>();
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddSingleton<ICalculationService, CecCountingService>();

        return services;
    }
}