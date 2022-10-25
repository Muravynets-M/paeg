using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IVotingCentreProvider
{
    public PrivateVotingCentre VotingCentre { get; }

    public void SaveVote(UserVote userVote);

    public void CountVote(VoteResult voteResult);

    public bool HasBallotBeenUsed(string ballot);

    public IEnumerable<UserVote> GetVotes();

    public IEnumerable<VoteResult> GetVoteResults();
}