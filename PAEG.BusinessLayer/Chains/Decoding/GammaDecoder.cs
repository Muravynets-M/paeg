using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding;

public class GammaDecoder : IDecodingChain {
    private IDecodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public GammaDecoder(IDecodingChain? next, ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        var secret = userSecret.D;
        for (var i = 0; i < userVote.EncryptedVote.Length; i++)
        {
            userVote.EncryptedVote[i] = (byte) (userVote.EncryptedVote[i] ^ secret[i]);
        }

        _tableProvider.GetDecodingByIdBallot(userVote.IdBallot).Gamma = userVote.EncryptedVote;
        
        _next?.Decode(userVote, userSecret, votingCentre);
    }
}