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
        using var rsa = RSACryptoServiceProvider.Create();
        rsa.ImportParameters(userSecret.RsaParameters);

        if (!rsa.VerifyData(BitConverter.GetBytes(userVote.IdBallot), userVote.Sign, HashAlgorithmName.SHA512,
            RSASignaturePadding.Pkcs1))
        {
            throw new InvalidSignException();
        }
        _tableProvider.GetDecodingByIdBallot(userVote.IdBallot).SignVerified = true;

        _next?.Decode(userVote, userSecret, votingCentre);
    }
}