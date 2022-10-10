using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryVotingDataProvider : IVotingCentreDataProvider {
    private static PrivateVotingCentre _votingCentre = null!;
    private static List<Guid> _processedBallots = new();
    private static List<VoteResult> _voteResults = new();


    private class UserVote {
        public string Email { get; set; }
        public Guid? Id { get; set; }
    }
    private static List<UserVote> _userVotes = new ();

    public InMemoryVotingDataProvider()
    {
        GenerateData();
    }

    private static void GenerateData()
    {
        using var rsa = RSA.Create();
        _votingCentre = new PrivateVotingCentre(rsa.ExportParameters(true));
    }

    public PrivateVotingCentre VotingCentre => _votingCentre;
    public void SaveProcessedBallot(Guid guid)
    {
        _processedBallots.Add(guid);
    }
 
    public void SaveUserVoting(string email, Guid? id)
    {
        if (id == null)
        {
            _userVotes.Add(new UserVote()
            {
                Email = email
            });
        }
        else
        {
            var xd = _userVotes.FirstOrDefault(v => v.Email == email);
            xd.Id = id;
        }
    }
    public bool HasUserVoted(string email)
    {
        return _userVotes.Exists(v => v.Email == email && v.Id != null);
    }
    public bool HasBallotBeenUsed(Guid guid)
    {
        return _processedBallots.Exists(g => g == guid);
    }
    public void SaveVoteResult(Guid guid, int candidate)
    {
        _voteResults.Add(new VoteResult(guid, candidate));
    }
    public IEnumerable<VoteResult> GetVoteResults()
    {
        return _voteResults;
    }
}