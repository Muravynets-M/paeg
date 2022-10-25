using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryVotingProvider : IVotingCentreProvider {
    private static PrivateVotingCentre _votingCentre = null!;
    private static List<UserVoteEntity> _userVotes = new();
    private static List<VoteResult> _voteResults = new();

    static InMemoryVotingProvider()
    {
        GenerateData();
    }

    private static void GenerateData()
    {
        using var dsa = DSA.Create();
        _votingCentre = new PrivateVotingCentre(dsa.ExportParameters(true));
    }

    public PrivateVotingCentre VotingCentre => _votingCentre;

    public void SaveVote(UserVote userVote)
    {
        _userVotes.Add(new UserVoteEntity(userVote));
    }

    public void CountVote(VoteResult voteResult)
    {
        _voteResults.Add(voteResult);
    }

    public bool HasBallotBeenUsed(string ballot)
    {
        return _voteResults.Exists(v => v.Ballot == ballot);
    }

    public IEnumerable<UserVote> GetVotes()
    {
        return _userVotes
            .OrderBy(v => v.DateTime)
            .Select(v => v.UserVote);
    }
    public IEnumerable<VoteResult> GetVoteResults()
    {
        return _voteResults;
    }
}