using PAEG.Model;

namespace PAEG.BusinessLayer.Services.Voting;

public interface IVotingService {
    List<SignedMaskedBallot> SendToVotingCenter(List<List<MaskedBallot>> maskedBallots, int maskKey, string email);
    void Vote(SignedEncodedBallot ballot);
    public static byte[] Unmask(byte[] secret, int mask)
    {
        var bytes = new List<byte>();
        var gamma = BitConverter.GetBytes(mask);
        
        for (var i = 0; i < secret.Length; i++)
        {
            bytes.Add((byte) (secret[i] ^ gamma[i % 4]));
        }

        return bytes.ToArray();
    }
}