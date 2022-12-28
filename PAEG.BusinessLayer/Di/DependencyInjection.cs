using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.BusinessLayer.Decryption;
using PAEG.BusinessLayer.Encryption;

namespace PAEG.BusinessLayer.Di;

public static class DependencyInjection {
    public static IServiceCollection AddBusinessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IEncryptionService, EncryptionService>();
        services.AddSingleton<IDecryptionService, DecryptionService>();

        return services;
    }
}