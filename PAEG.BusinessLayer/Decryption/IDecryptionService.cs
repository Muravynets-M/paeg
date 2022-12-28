using PAEG.Model;

namespace PAEG.BusinessLayer.Decryption;

public interface IDecryptionService
{
    public List<(CandidateData, int)> CalculateVotes();
}