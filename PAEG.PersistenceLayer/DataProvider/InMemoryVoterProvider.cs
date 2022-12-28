using System.Data;
using System.Security.Cryptography;
using ElGamalExt;
using PAEG.Encryption;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryVoterProvider : IVoterProvider {
    private static readonly List<UserPrivateData> Data = new();

    public InMemoryVoterProvider(int userAmount)
    {
        for (var i = 1; i <= userAmount; i++)
        {
            using var elGamal = new ElGamalManaged();
            Data.Add(new UserPrivateData(
                    Faker.Name.First(),
                    Faker.RandomNumber.Next().ToString(),
                    new Token(i, new BbsParameters(BlumBlumSnub.N), elGamal)
                )
            );
        }
    }

    public UserPrivateData GetUnregisteredPrivateUserData()
    {
        var data = Data.FirstOrDefault(u => u.SerialNumber is null);
        if (data is null)
            throw new DataException();

        return data;
    }

    public IEnumerable<UserPrivateData> GetRegisteredPrivateUserData()
    {
        return Data.Where(u => u.SerialNumber is not null).ToList();
    }

    public UserPrivateData? GetPrivateUserDataById(int id)
    {
        return Data
            .FirstOrDefault(userData => userData.Token.Id == id);
    }
    
    public UserPrivateData? GetPrivateUserDataByLoginPassword(string login, string password)
    {
        return Data
            .FirstOrDefault(userData => userData.Login == login && userData.Password == password);
    }
}