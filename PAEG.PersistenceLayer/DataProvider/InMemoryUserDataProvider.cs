using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryUserDataProvider : IUserDataProvider
{
    private static readonly List<UserDataEntity> Data = new();

    public InMemoryUserDataProvider(int userAmount, int factoring)
    {
        GenerateData(userAmount, factoring);
    }

    private static void GenerateData(int userAmount, int factoring)
    {
        for (var i = 1; i <= userAmount; i++)
        {
            using var rsa = RSA.Create();
            Data.Add(new UserDataEntity(Faker.Internet.Email(), (int)Math.Pow(10,  factoring) * i, rsa.ExportParameters(true)));
        }
    }

    public IEnumerable<UserPrivateData> GetPrivateUserData()
    {
        return Data.Select(user => new UserPrivateData(
            user.Email,
            user.RsaParameters.Modulus!,
            user.RsaParameters.Exponent!,
            user.RsaParameters.D!,
            user.IdBallot,
            user.RsaParameters)
        ).ToList();
    }

    public IEnumerable<UserData> GetPublicUserData()
    {
        return Data.Select(user => new UserData(
            user.Email,
            user.RsaParameters.Modulus!,
            user.RsaParameters.Exponent!)
        ).ToList();
    }
    public UserPrivateData? GetPrivateUserDataByEmail(string email)
    {
        return Data
            .Where(userData => userData.Email == email)
            .Select(user => new UserPrivateData(
            user.Email,
            user.RsaParameters.Modulus!,
            user.RsaParameters.Exponent!,
            user.RsaParameters.D!,
            user.IdBallot,
            user.RsaParameters)
            ).FirstOrDefault();
    }
}