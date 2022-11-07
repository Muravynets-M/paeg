using PAEG.Model;

namespace PAEG.BusinessLayer.Voter; 

public interface IEncryptionService {
    public const int RandomStringLength = 4;
    public EncryptedBallot EncryptVote(int idVoter, byte[] vote);
}