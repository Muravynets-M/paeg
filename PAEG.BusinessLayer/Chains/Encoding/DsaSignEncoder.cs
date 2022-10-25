using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class DsaSignEncoder : IEncodingChain {
    private readonly IEncodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public DsaSignEncoder(IEncodingChain? next,
        ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        using var dsa = DSACryptoServiceProvider.Create();
        dsa.ImportParameters(userSecret.DsaParameters);

        var signedVote = dsa.SignData(
            userVote.EncryptedVote,
            HashAlgorithmName.SHA512,
            DSASignatureFormat.Rfc3279DerSequence
        );
        _tableProvider.GetEncodingByIdBallot(userVote.Ballot).SignedHash = signedVote;
        userVote.Sign = signedVote;

        _next?.Encode(userVote, userSecret);
    }
}