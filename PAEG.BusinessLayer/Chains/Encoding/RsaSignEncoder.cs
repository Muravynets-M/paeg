using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class RsaSignEncoder : IEncodingChain {
    private readonly IEncodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public RsaSignEncoder(IEncodingChain? next,
        ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        using var rsa = RSACryptoServiceProvider.Create();
        rsa.ImportParameters(userSecret.RsaParameters);

        var signedVote = rsa.SignData(BitConverter.GetBytes(userVote.IdBallot), HashAlgorithmName.SHA512,
        RSASignaturePadding.Pkcs1);
        _tableProvider.GetEncodingByIdBallot(userVote.IdBallot).SignedHash = signedVote;
        userVote.Sign = signedVote;

        _next?.Encode(userVote, userSecret);
    }
}