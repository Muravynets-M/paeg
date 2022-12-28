using System.Collections;
using PAEG.Encryption;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Decryption;

public class DecryptionService: IDecryptionService
{
    private readonly ICandidateProvider _candidateProvider;
    private readonly IBallotsProvider _ballotsProvider;
    private readonly IVoterProvider _voterProvider;

    public DecryptionService(
        ICandidateProvider candidateProvider,
        IBallotsProvider ballotsProvider,
        IVoterProvider voterProvider
    )
    {
        _candidateProvider = candidateProvider;
        _ballotsProvider = ballotsProvider;
        _voterProvider = voterProvider;
    }

    public List<(CandidateData, int)> CalculateVotes()
    {
        var candidateVotes = _candidateProvider.GetAllCandidates().Select(c => (c, 0)).ToArray();
        var ballots = _ballotsProvider.GetAllBallots();
        foreach (var ballot in ballots)
        {
            var elGamal = _voterProvider.GetPrivateUserDataById(ballot.Id).Token.ElGamalParams;
            var decryptedVote = elGamal.DecryptData(ballot.Bytes);
            var bits = new BitArray(decryptedVote);

            var x = ballot.X0;
            for (int i = 0; i < bits.Count; i++)
            {
                bits[i] ^= BlumBlumSnub.Lsb(x);
                x = BlumBlumSnub.Next(x);
            }

            var vote = BitConverter.ToInt32(BitArrayToByteArray(bits));
            
            candidateVotes[vote - 1] = (candidateVotes[vote - 1].c, candidateVotes[vote - 1].Item2 + 1);
        }

        return candidateVotes.ToList();
    }
    
    private static byte[] BitArrayToByteArray(BitArray bits)
    {
        byte[] ret = new byte[(bits.Length - 1) / 8 + 1];
        bits.CopyTo(ret, 0);
        return ret;
    }
}