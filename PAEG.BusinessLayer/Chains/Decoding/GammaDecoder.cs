using Microsoft.Extensions.Logging;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding;

public class GammaDecoder : IDecodingChain {
    private IDecodingChain? _next;
    private readonly ITableProvider _tableProvider;
    private readonly ILogger _logger;

    public GammaDecoder(IDecodingChain? next, ITableProvider tableProvider, ILogger<GammaDecoder> logger)
    {
        _next = next;
        _tableProvider = tableProvider;
        _logger = logger;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        var secret = BitConverter.GetBytes(Int32.MaxValue);
        _tableProvider.GetDecodingByIdBallot(userVote.IdBallot).Gamma = userVote.EncryptedVote.Select(b => b).ToArray();
        
        for (var i = 0; i < userVote.EncryptedVote.Length; i++)
        {
            userVote.EncryptedVote[i] ^= secret[i];;
        }
        
        _tableProvider.GetDecodingByIdBallot(userVote.IdBallot).Bytes = userVote.EncryptedVote.Select(b => b).ToArray();
        _next?.Decode(userVote, userSecret, votingCentre);
    }
}