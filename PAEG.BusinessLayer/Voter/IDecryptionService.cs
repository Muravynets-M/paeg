using PAEG.Model;

namespace PAEG.BusinessLayer.Voter; 

public interface IDecryptionService {
    public IEnumerable<EncryptedBallot> DecryptVote(int idVoter, IEnumerable<EncryptedBallot> ballots);
    public IEnumerable<SignedBallot> SignVote(int idVoter, IEnumerable<SignedBallot> ballots);
}