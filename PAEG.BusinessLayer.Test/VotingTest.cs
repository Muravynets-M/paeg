using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PAEG.BusinessLayer.Encryption;
using PAEG.BusinessLayer.Voter;
using PAEG.DI;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Test;

public class VotingTest
{

    private IEcSendingService _ecSendingService;
    private IEncryptionService _encryptionService;
    private ICalculationService _calculationService;

    private ICandidateProvider _candidateProvider;
    private IVoterProvider _voterProvider;
    private IEcProvider _ecProvider;
    
    private readonly IDictionary<int, int> _votes = new Dictionary<int, int>();

    public VotingTest()
    {
        BuildDependencies();
        var candidates = _candidateProvider.GetAllCandidates();

        _votes[1] = candidates[0].Id;
        _votes[2] = candidates[1].Id;
        _votes[3] = candidates[0].Id;
        _votes[4] = candidates[0].Id;
    }

    [Fact]
    public void TestRsaHomomorphic()
    {
        var a = 2;
        var b = 16;

        var aEncoded = HardcodedRsa.Encrypt(a);
        var bEncoded = HardcodedRsa.Encrypt(b);
        
        var aTimesBEncoded = HardcodedRsa.Encrypt(a * b);

        var homomorphicDecryption = HardcodedRsa.Decrypt(aEncoded * bEncoded);
        var simpleDecryption = HardcodedRsa.Decrypt(aTimesBEncoded);    
        
        Assert.Equal(simpleDecryption, homomorphicDecryption);
        Assert.Equal(a*b, homomorphicDecryption);
    }

    [Fact]
    public void TestVotingSuccess()
    {
        var encryptedVotes = _votes
            .Select((pair => _encryptionService.EncryptAndSplitBallots(pair.Key, pair.Value)))
            .ToList();

        var ecs = _ecProvider.GetAllEcs();

        foreach (var votes in encryptedVotes)
        {
            for (var i = 0; i < votes.Count; i++)
            {
                _ecSendingService.SendToEc(ecs[i].Id, votes[i]);
            }
        }

        var results = _calculationService.CalculateVotes();

        foreach (var result in results)
        {
            Assert.Equal(_votes[result.Item1], result.Item2);
        }
    }

    [Fact]
    public void TestUserTriesToVoteAgain()
    {
        var encryptedVotes = _votes
            .Select((pair => _encryptionService.EncryptAndSplitBallots(pair.Key, pair.Value)))
            .ToList();

        var ecs = _ecProvider.GetAllEcs();

        foreach (var votes in encryptedVotes)
        {
            for (var i = 0; i < votes.Count; i++)
            {
                _ecSendingService.SendToEc(ecs[i].Id, votes[i]);
            }
        }
        
        Exception
    }

    private void BuildDependencies()
    {

        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .Build();

        services.AddApiDependencies(configuration);

        var serviceProvider = services.BuildServiceProvider();

        _ecSendingService = serviceProvider.GetService<IEcSendingService>()!;
        _encryptionService = serviceProvider.GetService<IEncryptionService>()!;
        _calculationService = serviceProvider.GetService<ICalculationService>()!;
        _voterProvider = serviceProvider.GetService<IVoterProvider>()!;
        _candidateProvider = serviceProvider.GetService<ICandidateProvider>()!;
        _ecProvider = serviceProvider.GetService<IEcProvider>()!;
    }
}