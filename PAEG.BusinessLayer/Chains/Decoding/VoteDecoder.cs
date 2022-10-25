using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding; 

public class VoteDecoder: IDecodingChain {
    private readonly ITableProvider _tableProvider;
    private readonly IVotingCentreProvider _votingCentreProvider;
    private readonly int _candidateCount;

    public VoteDecoder(ITableProvider tableProvider, IVotingCentreProvider votingCentreProvider, int candidateCount)
    {
        _tableProvider = tableProvider;
        _votingCentreProvider = votingCentreProvider;
        _candidateCount = candidateCount;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        var vote = int.Parse(BitConverter.ToInt32(userVote.EncryptedVote).ToString());
        _tableProvider.GetDecodingByIdBallot(userVote.Ballot).Vote = vote;

        if (vote > _candidateCount || vote < 1)
        {
            throw new InvalidCandidateException();
        }

        _votingCentreProvider.CountVote(new VoteResult(userVote.Ballot, userVote.Identification, vote));
    }
}