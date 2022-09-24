using PAEG.Model;

namespace PAEG.BusinessLayer.Chains.Decoding;

public interface IDecodingChain
{
    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre);
}