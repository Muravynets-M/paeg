using PAEG.Model;

namespace PAEG.BusinessLayer.Voter;

public interface IEncryptionService
{
    public List<SignedBallot> EncryptAndSplitBallots(int idVoter, int candidate);
}