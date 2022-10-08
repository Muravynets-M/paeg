using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Security.Cryptography;

namespace PAEG.BusinessLayer.Chains.Decoding;

public class RsaDecodeDecoder : IDecodingChain {
    private IDecodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public RsaDecodeDecoder(IDecodingChain? next, ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        using var rsa = RSACryptoServiceProvider.Create();

        rsa.ImportParameters(votingCentre.RsaParameters);
        userVote.EncryptedVote = rsa.Decrypt(userVote.EncryptedVote, RSAEncryptionPadding.Pkcs1);

        _tableProvider.GetDecodingByIdBallot(userVote.IdBallot).DecryptedHash = userVote.EncryptedVote.Select(b => b).ToArray();

        _next?.Decode(userVote, userSecret, votingCentre);
    }
}