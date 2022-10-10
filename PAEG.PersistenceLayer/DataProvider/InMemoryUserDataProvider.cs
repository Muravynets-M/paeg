using System.Security.Cryptography;
using PAEG.Model;
using PAEG.Model.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryUserDataProvider : IUserDataProvider {
    private static readonly List<UserData> Data = new();

    public InMemoryUserDataProvider(int userAmount, int factoring, int packSize)
    {
        GenerateData(userAmount, factoring, packSize);
    }

    private static void GenerateData(int userAmount, int candidateAmount, int packSize)
    {
        for (var i = 1; i <= userAmount; i++)
        {
            using var rsa = RSA.Create();
            var packs = new List<List<Ballot>>();
            for (int j = 0; j < packSize; j++)
            {
                
                packs.Add(new List<Ballot>());
                for (int k = 1; k <= candidateAmount+1; k++)
                {
                    packs[j].Add(new Ballot(k, Guid.NewGuid()));
                }
            }
            Data.Add(
                new UserData(
                    Faker.Internet.Email(),
                    rsa.ExportParameters(true),
                    packs
                )
            );
        }
    }

    public IEnumerable<UserData> GetUserData()
    {
        return Data;
    }


    public UserData? GetUserDataByEmail(string email)
    {
        return Data.FirstOrDefault(u => u.Email == email);
    }
}