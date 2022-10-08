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
        foreach (var encdoingTable in _tableProvider.GetAllEncodingTables())
        {
            var decodingTable = _tableProvider.GetDecodingByIdBallot(encdoingTable.IdBallot);
            var user = _userDataProvider.GetPrivateUserData().FirstOrDefault(u => u.IdBallot == encdoingTable.IdBallot);
            if (user is null)
            {
                decodingTable.Exception = "InvalidIdBallot";
                continue;
            }
            foreach (var vote in _votingCentreDataProvider.GetVotesByIdBallotOrdered(user.IdBallot))
            {
                try
                {
                    _decodingChain.Decode(vote, user, _votingCentreDataProvider.VotingCentre);
                }
                catch (BusinessException exception)
                {
                    decodingTable.Exception = exception.ToString();
                }
            }
        }

    }
}