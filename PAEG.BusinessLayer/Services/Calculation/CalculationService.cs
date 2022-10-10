using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Services.Calculation;

public class CalculationService : ICalculationService {
    public static bool HasVotingEnded { get; private set; }

    private IVotingCentreDataProvider _votingCentreProvider;
    
    public CalculationService(IVotingCentreDataProvider votingCentreProvider)
    {
        _votingCentreProvider = votingCentreProvider;
    }
    
    public void CalculateVotes()
    {
        HasVotingEnded = true;
    }
    
    public IEnumerable<VoteResult> GetVoteResults()
    {
        if (!HasVotingEnded)
        {
            throw new VotingHasntEnded();
        }

        return _votingCentreProvider.GetVoteResults();
    }
}