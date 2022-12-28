using PAEG.Model;

namespace PAEG.BusinessLayer.Encryption;

public interface IEncryptionService
{
    public EncryptedBallot EncryptVote(UserPrivateData userData, CandidateData candidate);
}