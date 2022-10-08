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
        var secret = BitConverter.GetBytes(Int32.MaxValue);
        _tableProvider.GetEncodingByIdBallot(userVote.IdBallot).Bytes = userVote.EncryptedVote.Select(b => b).ToArray();
        
        for (var i = 0; i < userVote.EncryptedVote.Length; i++)
        {
            userVote.EncryptedVote[i] ^= secret[i];
        }

        _tableProvider.GetEncodingByIdBallot(userVote.IdBallot).Gamma = userVote.EncryptedVote.Select(b => b).ToArray();

        _next?.Encode(userVote, userSecret);
    }
}