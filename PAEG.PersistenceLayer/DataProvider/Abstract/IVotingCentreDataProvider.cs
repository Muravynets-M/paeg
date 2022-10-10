using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IVotingCentreDataProvider
{
    public PrivateVotingCentre VotingCentre { get; }

    public void SaveProcessedBallot(Guid guid);

    public void SaveUserVoting(string email, Guid? id = null);

    public bool HasUserVoted(string email);

    public bool HasBallotBeenUsed(Guid guid);

    public void SaveVoteResult(Guid guid, int candidate);

    public IEnumerable<VoteResult> GetVoteResults();
}