using PAEG.Model;

namespace PAEG.BusinessLayer.Services.Calculation;

public interface ICalculationService
{
    public void CalculateVotes();
    public IEnumerable<VoteResult> GetVoteResults();
}