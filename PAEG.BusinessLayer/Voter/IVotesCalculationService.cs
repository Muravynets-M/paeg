using PAEG.Model;

namespace PAEG.BusinessLayer.Voter; 

public interface IVotesCalculationService {
    public int CalculateVote(int idVoter, IEnumerable<SignedBallot> ballots);
}