using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryVoterProvider : IVoterProvider {
    private static readonly List<UserPrivateData> Data = new();

    public InMemoryVoterProvider(int userAmount)
    {
        for (var i = 1; i <= userAmount; i++)
        {
            using var dsa = RSACryptoServiceProvider.Create();
            Data.Add(new UserPrivateData(
                    i,
                    dsa.ExportParameters(true)
                )
            );
        }
    }

    public IEnumerable<UserPrivateData> GetPrivateUserData()
    {
        return Data.ToList();
    }

    public UserPrivateData? GetPrivateUserDataById(int id)
    {
        return Data
            .FirstOrDefault(userData => userData.Id == id);
    }
}