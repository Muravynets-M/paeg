using System.Security.Cryptography;
using PAEG.BusinessLayer.Encryption;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Voter;

public class CecCountingService: ICalculationService
{
    private IVoterProvider _voterProvider;
    private IEcProvider _ecProvider;
    private ICecProvider _cecProvider;

    public CecCountingService(IVoterProvider voterProvider, IEcProvider ecProvider, ICecProvider cecProvider)
    {
        _voterProvider = voterProvider;
        _ecProvider = ecProvider;
        _cecProvider = cecProvider;
    }

    public List<(int, int)> CalculateVotes()
    {
        return _voterProvider.GetPrivateUserData()
            .Select(data => (data.Id, _ecProvider.GetAllUserBallots(data.Id)))
            .Select(ballots => 
                (ballots.Id, ballots.Item2.Aggregate(1, (rez, ballot) => rez * ballot.Ballot)))
            .Select(ballots => ballots with { Item2 = HardcodedRsa.Decrypt(ballots.Item2)})
            .ToList();
    }
}