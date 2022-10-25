using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class TerminalEncoder: IEncodingChain {
    private IVotingCentreProvider _votingCentreProvider;

    public TerminalEncoder(IVotingCentreProvider votingCentreProvider)
    {
        _votingCentreProvider = votingCentreProvider;
    }
    
    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        _votingCentreProvider.SaveVote(userVote);
    }
}