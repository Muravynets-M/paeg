using ElGamalExt;
using PAEG.BusinessLayer.Chains.Secrets;
using System.Collections;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class ElGamalEncoder : IEncodingChain {
    private readonly IEncodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public ElGamalEncoder(
        IEncodingChain? next,
        ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        userVote.EncryptedVote = ElGamalSecret.ElGamal.EncryptData(userVote.EncryptedVote);
        
        _tableProvider.GetEncodingByIdBallot(userVote.Ballot).ElGamal = userVote.EncryptedVote.Select(b => b).ToArray();

        _next?.Encode(userVote, userSecret);
    }
}