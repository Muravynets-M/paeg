using Microsoft.Extensions.Logging;
using PAEG.BusinessLayer.Chains.Secrets;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding;

public class ElGamalDecoder : IDecodingChain {
    private IDecodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public ElGamalDecoder(IDecodingChain? next, ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        userVote.EncryptedVote = ElGamalSecret.ElGamal.DecryptData(userVote.EncryptedVote);
        
        _tableProvider.GetDecodingByIdBallot(userVote.Ballot).DecryptedHash = userVote.EncryptedVote.Select(b => b).ToArray();
        _next?.Decode(userVote, userSecret, votingCentre);
    }
}