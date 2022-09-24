using PAEG.Model;

namespace PAEG.BusinessLayer.Chains.Encoding;

public interface IEncodingChain
{
    public void Encode(UserVote userVote, UserPrivateData userSecret);
}