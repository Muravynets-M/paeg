using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Chains.Decoding; 

public class BallotDecoder: IDecodingChain {
    private readonly IDecodingChain? _next;
    private readonly IVotingCentreProvider _votingCentreProvider;
    private readonly IRegistrationProvider _registrationProvider;
    
    public BallotDecoder(IDecodingChain? next, IVotingCentreProvider votingCentreProvider, IRegistrationProvider registrationProvider)
    {
        _votingCentreProvider = votingCentreProvider;
        _registrationProvider = registrationProvider;
        _next = next;
    }
    public void Decode(UserVote userVote, UserPrivateData userSecret, PrivateVotingCentre votingCentre)
    {
        if (_votingCentreProvider.HasBallotBeenUsed(userVote.Ballot))
        {
            throw new BallotAlreadyUsedException();
        }

        if (!_registrationProvider.IsRegistered(userVote.Ballot))
        {
            throw new BallotWasntRegisteredException();
        }
        
        _next?.Decode(userVote, userSecret, votingCentre);
    }
}