using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Security.Cryptography;

namespace PAEG.BusinessLayer.Voter; 

public class EncryptionService: IEncryptionService {
    private readonly IVoterProvider _voterProvider;
    private readonly IVoterRandomStringsProvider _stringsProvider;
    
    public EncryptionService(IVoterProvider voterProvider, IVoterRandomStringsProvider stringsProvider)
    {
        _voterProvider = voterProvider;
        _stringsProvider = stringsProvider;
    }
    
    public EncryptedBallot EncryptVote(int idVoter, byte[] vote)
    {
        var (encryptedVote, randomStringBytes) = AddStringToBytes(Faker.Internet.UserName(), vote);
        _stringsProvider.Save(idVoter, 0, randomStringBytes);

        foreach (var voters in _voterProvider.GetPrivateUserData())
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(voters.RsaParametersShort);

            encryptedVote = rsa.Encrypt(encryptedVote, RSAEncryptionPadding.Pkcs1);
        }

        var i = 1;
        foreach (var voter in _voterProvider.GetPrivateUserData())
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(voter.RsaParametersLong);

            (encryptedVote, var randomString) = AddStringToBytes(Faker.Internet.UserName(), encryptedVote);
            _stringsProvider.Save(idVoter, i++, randomString);

            encryptedVote = rsa.Encrypt(encryptedVote, RSAEncryptionPadding.Pkcs1);
        }

        return new EncryptedBallot {IdBallot = idVoter, Ballot = encryptedVote};
    }

    private (byte[], byte[])  AddStringToBytes(string stringToAdd, byte[] bytes)
    {
        var stringBytes = stringToAdd
            .SelectMany(BitConverter.GetBytes)
            .Take(IEncryptionService.RandomStringLength)
            .ToArray();

        return (bytes.Concat(stringBytes).ToArray(), stringBytes);
    }
}