using PAEG.BusinessLayer.Exceptions;
using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding;

public class RsaSignDecoder : IDecodingChain {
    private readonly IDecodingChain? _next;
    private readonly ITableProvider _tableProvider;

    public RsaSignDecoder(IDecodingChain? next, ITableProvider tableProvider)
    {
        _next = next;
        _tableProvider = tableProvider;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        using var dsa = DSACryptoServiceProvider.Create();
        dsa.ImportParameters(userSecret.DsaParameters);

        if (!dsa.VerifyData(
                userVote.EncryptedVote,
                userVote.Sign, HashAlgorithmName.SHA512,
                DSASignatureFormat.Rfc3279DerSequence)
           )
        {
            throw new InvalidSignException();
        }
        
        _tableProvider.GetDecodingByIdBallot(userVote.Ballot).SignVerified = true;

        _next?.Decode(userVote, userSecret, votingCentre);
    }
}