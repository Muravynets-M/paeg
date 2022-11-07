using ElGamalExt;
using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryVoterProvider : IVoterProvider {
    private static readonly List<UserDataEntity> Data = new();

    public InMemoryVoterProvider(int userAmount, int factoring)
    {
        GenerateData(userAmount);
    }

    private static void GenerateData(int userAmount)
    {
        for (var i = 1; i <= userAmount; i++)
        {
            using var rsa512 = RSACryptoServiceProvider.Create(1024 + 512 * (i-1));
            using var rsa1024 = RSACryptoServiceProvider.Create(3072 + 512 * (i-1));
            using var elGamal = new ElGamalManaged();
            Data.Add(new UserDataEntity(
                    i,
                    rsa512.ExportParameters(true),
                    rsa1024.ExportParameters(true),
                    elGamal.ExportParameters(true)
                )
            );
        }
    }

    public IEnumerable<UserPrivateData> GetPrivateUserData()
    {
        return Data.Select(
            user => new UserPrivateData(
                user.Id,
                user.RsaParametersShort,
                user.RsaParametersLong,
                user.ElGamalParameters
            )
        ).ToList();
    }

    public UserPrivateData? GetPrivateUserDataById(int id)
    {
        return Data
            .Where(userData => userData.Id == id)
            .Select(
                user => new UserPrivateData(
                    user.Id,
                    user.RsaParametersShort,
                    user.RsaParametersLong,
                    user.ElGamalParameters
                )
            ).FirstOrDefault();
    }
}