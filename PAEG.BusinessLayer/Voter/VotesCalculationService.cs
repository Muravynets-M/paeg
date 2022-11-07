using ElGamalExt;
using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Security.Cryptography;

namespace PAEG.BusinessLayer.Voter;

public class VotesCalculationService : IVotesCalculationService {
    private readonly IVoterProvider _voterProvider;
    private readonly IVoterRandomStringsProvider _stringsProvider;
    private int _votersCount;

    public VotesCalculationService(IVoterProvider voterProvider, IVoterRandomStringsProvider stringsProvider, int votersCount)
    {
        _voterProvider = voterProvider;
        _stringsProvider = stringsProvider;
        _votersCount = votersCount;
    }

    public int CalculateVote(int idVoter, IEnumerable<SignedBallot> ballots)
    {
        var lastVoter = _voterProvider.GetPrivateUserDataById(1)!;
        var signedBallots = ballots.ToList();

        foreach (var ballot in signedBallots)
        {
            using var rsa = RSA.Create();
            rsa.ImportParameters(lastVoter.RsaParametersShort);
                
            // Doesn't work in this lib
            //var elGamal = new ElGamalManaged();
            //elGamal.ImportParameters(previousVoter.ElGamalParameters);
            if (!rsa.VerifyData(ballot.Ballot, ballot.Sign!, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1))
            {
                throw new InvalidSignException();
            }
        }

        var personalRandomString = _stringsProvider.GetByIdAndOrder(idVoter, 0);
        var vote = signedBallots
            .Select(b => b.Ballot)
            .FirstOrDefault(b =>
                b.TakeLast(IEncryptionService.RandomStringLength).SequenceEqual(personalRandomString)
            );
        if (vote == null)
        {
            throw new BallotIsNotPresentException();
        }

        return BitConverter.ToInt32(vote.AsSpan()[..^IEncryptionService.RandomStringLength]);
    }
}