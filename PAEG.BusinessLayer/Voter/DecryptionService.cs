using ElGamalExt;
using ElGamalExt.Homomorphism;
using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Diagnostics;
using System.Security.Cryptography;

namespace PAEG.BusinessLayer.Voter;

public class DecryptionService : IDecryptionService {
    private readonly IVoterProvider _voterProvider;
    private readonly IVoterRandomStringsProvider _stringsProvider;
    private static readonly Random Rng = new Random();

    private int _votersCount;

    public DecryptionService(IVoterProvider voterProvider, IVoterRandomStringsProvider stringsProvider, int votersCount)
    {
        _voterProvider = voterProvider;
        _stringsProvider = stringsProvider;
        _votersCount = votersCount;
    }

    public IEnumerable<EncryptedBallot> DecryptVote(int idVoter, IEnumerable<EncryptedBallot> ballots)
    {
        var encryptedBallots = ballots.ToList();
        if (encryptedBallots.Count != _votersCount)
        {
            throw new InvalidBallotCountException();
        }
        
        using var rsa = RSA.Create();
        var voter = _voterProvider.GetPrivateUserDataById(idVoter)!;
        rsa.ImportParameters(voter.RsaParametersLong);
        
        foreach (var ballot in encryptedBallots)
        {
            ballot.Ballot = rsa.Decrypt(ballot.Ballot, RSAEncryptionPadding.Pkcs1);
        }

        var randomString = _stringsProvider.GetByIdAndOrder(idVoter, idVoter);
        var isOwnBallotPresent = encryptedBallots
            .Select(b => b.Ballot.TakeLast(IEncryptionService.RandomStringLength).ToArray())
            .Any(b => CompareArrays(b, randomString));
        if (!isOwnBallotPresent)
        {
            throw new BallotIsNotPresentException();
        }

        foreach (var ballot in encryptedBallots)
        {
            ballot.Ballot = ballot.Ballot[..^IEncryptionService.RandomStringLength];
        }

        return Shuffle(encryptedBallots);
    }
    private bool CompareArrays(byte[] bytes, byte[] randomString)
    {
        for (var i = 0; i < randomString.Length; i++)
        {
            if (bytes[i] != randomString[i])
                return false;
        }

        return true;
    }

    public IEnumerable<SignedBallot> SignVote(int idVoter, IEnumerable<SignedBallot> ballots)
    {
        var encryptedBallots = ballots.ToList();
        if (!IsBallotPresent(idVoter, encryptedBallots))
        {
            throw new BallotIsNotPresentException();
        }

        if (!encryptedBallots.All(b => b.Sign is null))
        {
            var previousVoter = _voterProvider.GetPrivateUserDataById(idVoter + 1)!;
            foreach (var ballot in encryptedBallots)
            {
                using var rsa = RSA.Create();
                rsa.ImportParameters(previousVoter.RsaParametersShort);
                
                // Doesn't work in this lib
                //var elGamal = new ElGamalManaged();
                //elGamal.ImportParameters(previousVoter.ElGamalParameters);
                if (!rsa.VerifyData(ballot.Ballot, ballot.Sign!, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1))
                {
                    throw new InvalidSignException();
                }
            }
        }

        var voter = _voterProvider.GetPrivateUserDataById(idVoter)!;
        foreach (var ballot in encryptedBallots)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(voter.RsaParametersShort);
            // Doesn't work in this lib
            //using var elGamal = new ElGamalManaged();
            //elGamal.ImportParameters(voter.ElGamalParameters);
            
            ballot.Ballot = rsa.Decrypt(ballot.Ballot, RSAEncryptionPadding.Pkcs1);
            ballot.Sign = rsa.SignData(ballot.Ballot, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }

        return Shuffle(encryptedBallots);
    }

    private bool IsBallotPresent(int idVoter, IEnumerable<EncryptedBallot> ballots)
    {
        return ballots.Any(b => b.IdBallot == idVoter);
    }
    
    private static IEnumerable<T> Shuffle<T>(IEnumerable<T> list)
    {
        return list.OrderBy(_ => Rng.Next());
    }
}