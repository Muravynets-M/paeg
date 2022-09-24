using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Encoding;

public class TerminalEncoder: IEncodingChain {
    private IVotingCentreDataProvider _votingCentreDataProvider;

    public TerminalEncoder(IVotingCentreDataProvider votingCentreDataProvider)
    {
        _votingCentreDataProvider = votingCentreDataProvider;
    }
    
    public void Encode(UserVote userVote, UserPrivateData userSecret)
    {
        _votingCentreDataProvider.SaveVote(userVote);
    }
}