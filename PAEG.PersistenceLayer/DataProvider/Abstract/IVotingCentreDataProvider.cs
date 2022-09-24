using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IVotingCentreDataProvider
{
    public PrivateVotingCentre VotingCentre { get; }

    public void SaveVote(UserVote userVote);

    public void CountVote(VoteResult voteResult);

    public bool HasBallotBeenUsed(int idBallot);

    public IEnumerable<UserVote> GetVotesByIdBallotOrdered(int idBallot);

    public IEnumerable<VoteResult> GetVoteResults();
}