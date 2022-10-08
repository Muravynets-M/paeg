using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding; 

public class VoteDecoder: IDecodingChain {
    private readonly ITableProvider _tableProvider;
    private readonly IVotingCentreDataProvider _votingCentreDataProvider;
    private readonly int _candidateCount;

    public VoteDecoder(ITableProvider tableProvider, IVotingCentreDataProvider votingCentreDataProvider, int candidateCount)
    {
        _tableProvider = tableProvider;
        _votingCentreDataProvider = votingCentreDataProvider;
        _candidateCount = candidateCount;
    }

    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        var vote = BitConverter.ToInt32(userVote.EncryptedVote).ToString();
        _tableProvider.GetDecodingByIdBallot(userVote.IdBallot).Vote = int.Parse(vote);

        if ($"{vote[.._candidateCount.ToString().Length]}" != userVote.IdBallot.ToString()[.._candidateCount.ToString().Length])
        {
            throw new InvalidBallotIdException();
        }
        
        if (charToInt(vote[_candidateCount.ToString().Length]) > _candidateCount || charToInt(vote[_candidateCount.ToString().Length]) < 1)
        {
            throw new InvalidCandidateException();
        }

        if (_votingCentreDataProvider.HasBallotBeenUsed(userVote.IdBallot))
        {
            throw new BallotAlreadyUsedException();
        }
        
        _votingCentreDataProvider.CountVote(new VoteResult(userVote.IdBallot, charToInt(vote[_candidateCount.ToString().Length])));
    }
    private static int charToInt(char c)
    {
        return c - '0';
    }
}