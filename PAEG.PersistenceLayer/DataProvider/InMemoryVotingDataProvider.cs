using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryVotingDataProvider : IVotingCentreDataProvider {
    private static PrivateVotingCentre _votingCentre = null!;
    private static List<UserVoteEntity> _userVotes = new();
    private static List<VoteResult> _voteResults = new();

    static InMemoryVotingDataProvider()
    {
        GenerateData();
    }

    private static void GenerateData()
    {
        using var rsa = RSA.Create();
        _votingCentre = new PrivateVotingCentre(rsa.ExportParameters(true));
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

    public bool HasBallotBeenUsed(int idBallot)
    {
        return _voteResults.Exists(v => v.IdBallot == idBallot);
    }

    public IEnumerable<UserVote> GetVotesByIdBallotOrdered(int idBallot)
    {
        return _userVotes
            .Where(v => v.UserVote.IdBallot == idBallot)
            .OrderBy(v => v.DateTime)
            .Select(v => v.UserVote);
    }
    public IEnumerable<VoteResult> GetVoteResults()
    {
        return _voteResults;
    }
}