using System.Collections;
using PAEG.Encryption;
using PAEG.Model;
using BigInt = System.Numerics.BigInteger;

namespace PAEG.BusinessLayer.Encryption;

public class EncryptionService: IEncryptionService
{
    public EncryptedBallot EncryptVote(UserPrivateData userData, CandidateData candidate)
    {
        var seed = BlumBlumSnub.Seeds[userData.Token.Id - 1];
        var x = BlumBlumSnub.Next(seed);
        var x0 = x;
        var voteBits = new BitArray(BitConverter.GetBytes(candidate.Id));
        for (int i = 0; i < voteBits.Count; i++)
        {
            voteBits[i] ^= BlumBlumSnub.Lsb(x);
            x = BlumBlumSnub.Next(x);
        }

        var bytes = BitArrayToByteArray(voteBits);
        var encryptedBytes= userData.Token.ElGamalParams.EncryptData(bytes);

        return new EncryptedBallot()
        {
            Id = userData.Token.Id,
            Bytes = encryptedBytes,
            X0 = x0
        };
    }
    
    private static byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }
}