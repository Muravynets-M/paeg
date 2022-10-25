using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryUserProvider : IUserProvider {
    private static readonly List<UserDataEntity> Data = new();

    public InMemoryUserProvider(int userAmount, int factoring)
    {
        GenerateData(userAmount);
    }

    private static void GenerateData(int userAmount)
    {
        for (var i = 1; i <= userAmount; i++)
        {
            using var dsa = DSA.Create();
            Data.Add(new UserDataEntity(
                    Faker.Internet.Email(),
                    $"#{Faker.Identification.MedicareBeneficiaryIdentifier()}",
                    dsa.ExportParameters(true)
                )
            );
        }
    }

    public IEnumerable<UserPrivateData> GetPrivateUserData()
    {
        return Data.Select(
            user => new UserPrivateData(
                user.Email,
                user.Identity,
                user.RsaParameters
            )
        ).ToList();
    }

    public UserPrivateData? GetPrivateUserDataByEmail(string email)
    {
        return Data
            .Where(userData => userData.Email == email)
            .Select(
                user => new UserPrivateData(
                    user.Email,
                    user.Identity,
                    user.RsaParameters
                )
            ).FirstOrDefault();
    }
}