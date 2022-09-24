using PAEG.BusinessLayer.Chains.Decoding;
using PAEG.BusinessLayer.Exceptions;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Services.Calculation;

public class CalculationService : ICalculationService {
    private readonly IUserDataProvider _userDataProvider;
    private readonly IVotingCentreDataProvider _votingCentreDataProvider;
    private readonly IDecodingChain _decodingChain;
    private readonly ITableProvider _tableProvider;

    public CalculationService(IVotingCentreDataProvider votingCentreDataProvider,
        IUserDataProvider userDataProvider,
        IDecodingChain decodingChain,
        ITableProvider tableProvider)
    {
        _votingCentreDataProvider = votingCentreDataProvider;
        _userDataProvider = userDataProvider;
        _decodingChain = decodingChain;
        _tableProvider = tableProvider;
    }

    public void CalculateVotes()
    {
        foreach (var user in _userDataProvider.GetPrivateUserData())
        {
            foreach (var vote in _votingCentreDataProvider.GetVotesByIdBallotOrdered(user.IdBallot))
            {
                try
                {
                    _decodingChain.Decode(vote, user, _votingCentreDataProvider.VotingCentre);
                }
                catch (BusinessException exception)
                {
                    _tableProvider.GetDecodingByIdBallot(vote.IdBallot).Exception = exception.ToString();
                }
            }
        }

    }
}