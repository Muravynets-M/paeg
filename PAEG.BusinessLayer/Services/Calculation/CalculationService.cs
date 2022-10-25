using PAEG.BusinessLayer.Chains.Decoding;
using PAEG.BusinessLayer.Exceptions;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Services.Calculation;

public class CalculationService : ICalculationService {
    private readonly IUserProvider _userProvider;
    private readonly IVotingCentreProvider _votingCentreProvider;
    private readonly IDecodingChain _decodingChain;
    private readonly ITableProvider _tableProvider;

    public CalculationService(IVotingCentreProvider votingCentreProvider,
        IUserProvider userProvider,
        IDecodingChain decodingChain,
        ITableProvider tableProvider)
    {
        _votingCentreProvider = votingCentreProvider;
        _userProvider = userProvider;
        _decodingChain = decodingChain;
        _tableProvider = tableProvider;
    }

    public void CalculateVotes()
    {
        foreach (var vote in _votingCentreProvider.GetVotes())
        {
            var decodingTable = _tableProvider.GetDecodingByIdBallot(vote.Ballot);
            var user = _userProvider.GetPrivateUserData().FirstOrDefault(u => u.Identitification == vote.Identification);
            if (user is null)
            {
                decodingTable.Exception = "InvalidIdentification";
                continue;
            }
            try
            {
                _decodingChain.Decode(vote, user, _votingCentreProvider.VotingCentre);
            }
            catch (BusinessException exception)
            {
                decodingTable.Exception = exception.ToString();
            }
        }
    }
}