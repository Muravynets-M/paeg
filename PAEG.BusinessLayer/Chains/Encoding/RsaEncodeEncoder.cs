using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class RsaEncodeEncoder : IEncodingChain {
    private readonly IEncodingChain? _next;
    private readonly ITableProvider _tableProvider;
    private readonly IVotingCentreDataProvider _votingCenterProvider;

    public RsaEncodeEncoder(IEncodingChain? next,
        ITableProvider tableProvider,
        IVotingCentreDataProvider votingCenterProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
        _votingCenterProvider = votingCenterProvider;
    }

    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        var votingPublicKey = _votingCenterProvider.VotingCentre;

        using var rsa = RSACryptoServiceProvider.Create();
        rsa.ImportParameters(votingPublicKey.RsaParameters);

        var encryptedVote = rsa.Encrypt(userVote.EncryptedVote, RSAEncryptionPadding.Pkcs1);
        _tableProvider.GetEncodingByIdBallot(userVote.IdBallot).EncryptedHash = encryptedVote;
        userVote.EncryptedVote = encryptedVote;

        _next?.Encode(userVote, userSecret);
    }
}