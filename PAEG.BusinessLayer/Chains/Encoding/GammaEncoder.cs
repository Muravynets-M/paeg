using System.Collections;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class GammaEncoder : IEncodingChain {
    private readonly IEncodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public GammaEncoder(
        IEncodingChain? next,
        ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        var secret = userSecret.D;
        for (var i = 0; i < userVote.EncryptedVote.Length; i++)
        {
            userVote.EncryptedVote[i] = (byte) (userVote.EncryptedVote[i] ^ secret[i]);
        }

        _tableProvider.GetEncodingByIdBallot(userVote.IdBallot).Gamma = userVote.EncryptedVote;

        _next?.Encode(userVote, userSecret);
    }
}