using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.BusinessLayer.Di;
using PAEG.BusinessLayer.Exceptions;
using PAEG.BusinessLayer.Voter;
using PAEG.DI;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Test;

public class VotingTest {
    private IEncryptionService _encryptionService;
    private IDecryptionService _decryptionService;
    private IVotesCalculationService _votesCalculationService;

    private IVoterProvider _voterProvider;

    private readonly IDictionary<int, int> _votes = new Dictionary<int, int>();

    public VotingTest()
    {
        BuildDependencies();

        _votes[1] = 1;
        _votes[2] = 2;
        _votes[3] = 1;
        _votes[4] = 1;
    }

    [Fact]
    public void TestSuccess()
    {
        var voters = _voterProvider.GetPrivateUserData().ToList();

        var encryptedVotes =
            from voter in voters
            let voteBytes = BitConverter.GetBytes(_votes[voter.Id])
            select _encryptionService.EncryptVote(voter.Id, voteBytes);

        voters.Reverse();
        encryptedVotes = voters
            .Aggregate(encryptedVotes, (current, voter) =>
                _decryptionService.DecryptVote(voter.Id, current));

        var signedBallots = voters.Aggregate(
            encryptedVotes
                .Select(vote => new SignedBallot {IdBallot = vote.IdBallot, Ballot = vote.Ballot}),
            (ballots, voter) => _decryptionService.SignVote(voter.Id, ballots)
        ).ToList();

        foreach (var voter in voters)
        {
            Assert.Equal(_votes[voter.Id], _votesCalculationService.CalculateVote(voter.Id, signedBallots));
        }
    }

    [Fact]
    public void Test_InvalidBallotCountException()
    {
        var voters = _voterProvider.GetPrivateUserData().ToList();

        var encryptedVotes =
            from voter in voters
            let voteBytes = BitConverter.GetBytes(_votes[voter.Id])
            select _encryptionService.EncryptVote(voter.Id, voteBytes);

        var encryptedBallots = encryptedVotes.ToList();
        encryptedBallots.Add(new EncryptedBallot() {IdBallot = 6, Ballot = new[] {(byte) 1}});

        voters.Reverse();
        Assert.Throws<InvalidBallotCountException>(() => voters
            .Aggregate((IEnumerable<EncryptedBallot>)encryptedBallots, (current, voter) =>
                _decryptionService.DecryptVote(voter.Id, current)
            )
        );
    }
    
    [Fact]
    public void Test_BallotIsNotPresentException()
    {
        var voters = _voterProvider.GetPrivateUserData().ToList();

        var encryptedVotes =
            from voter in voters
            let voteBytes = BitConverter.GetBytes(_votes[voter.Id])
            select _encryptionService.EncryptVote(voter.Id, voteBytes);

        var encryptedBallots = encryptedVotes.ToList();

        voters.Reverse();
        
        encryptedBallots[3].Ballot = encryptedBallots[2].Ballot;
        Assert.Throws<BallotIsNotPresentException>(() => voters
            .Aggregate((IEnumerable<EncryptedBallot>)encryptedBallots, (current, voter) =>
                _decryptionService.DecryptVote(voter.Id, current)
            )
        );
    }

    private void BuildDependencies()
    {

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();

        services.AddApiDependencies(configuration);

        var serviceProvider = services.BuildServiceProvider();
        _encryptionService = serviceProvider.GetService<IEncryptionService>()!;
        _decryptionService = serviceProvider.GetService<IDecryptionService>()!;
        _votesCalculationService = serviceProvider.GetService<IVotesCalculationService>()!;
        _voterProvider = serviceProvider.GetService<IVoterProvider>()!;
    }
}