using System.Security.Cryptography;
using PAEG.BusinessLayer.Encryption;
using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Voter;

public class EncryptionService : IEncryptionService
{
    private readonly IVoterProvider _voterProvider;
    private readonly IEcProvider _ecProvider;
    private readonly ICecProvider _cecProvider;

    public EncryptionService(IVoterProvider voterProvider, IEcProvider ecProvider, ICecProvider cecProvider)
    {
        _voterProvider = voterProvider;
        _ecProvider = ecProvider;
        _cecProvider = cecProvider;
    }

    public List<SignedBallot> EncryptAndSplitBallots(int idVoter, int candidate)
    {
        var voter = _voterProvider.GetPrivateUserDataById(idVoter)!;

        var factor = GetFactors(candidate);
        
        using var dsa = RSACryptoServiceProvider.Create();
        dsa.ImportParameters(voter.RsaParameters);
        var sign = dsa.SignData(
            BitConverter.GetBytes(idVoter),
            HashAlgorithmName.SHA1,
            RSASignaturePadding.Pkcs1
        );
        
        var signedBallots = new List<SignedBallot>
        {
            new SignedBallot
            {
                IdUser = idVoter,
                Sign = sign,
                Ballot = ManualRsa.Encrypt(factor),
            },
            new SignedBallot
            {
                IdUser = idVoter,
                Sign = sign,
                Ballot = ManualRsa.Encrypt(candidate/factor)
            }
        };

        return signedBallots;
    }

    private int GetFactors(int number)
    {
        for (var i = 2; i < number / 2; i++)
        {
            if (number % i == 0)
                return i;
        }

        throw new ArgumentException($"{number}");
    }
}