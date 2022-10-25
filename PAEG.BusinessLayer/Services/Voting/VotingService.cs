
using PAEG.BusinessLayer.Chains.Encoding;
using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Services.Voting;

public class VotingService : IVotingService
{
    private readonly IUserProvider _userProvider;
    private readonly IEncodingChain _encodingChain;
    private readonly ITableProvider _tableProvider;

    public VotingService(IEncodingChain encodingChain,
        ITableProvider tableProvider,
        IUserProvider userProvider)
    {
        _encodingChain = encodingChain;
        _tableProvider = tableProvider;
        _userProvider = userProvider;
    }

    public void Vote(string email, string identification, string ballot, int candidate)
    {
        var vote = BitConverter.GetBytes(candidate);
        var table = _tableProvider.GetEncodingByIdBallot(ballot);
        table.Vote = candidate;
        table.Identification = identification;

        _encodingChain.Encode(new UserVote(ballot) {EncryptedVote = vote, Identification = identification},
            _userProvider.GetPrivateUserDataByEmail(email) ?? throw new UserNotFoundException());
    }
}