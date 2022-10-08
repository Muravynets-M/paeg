
using PAEG.BusinessLayer.Chains.Encoding;
using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Services.Voting;

public class VotingService : IVotingService
{
    private readonly IUserDataProvider _userDataProvider;
    private readonly IEncodingChain _encodingChain;
    private readonly ITableProvider _tableProvider;

    public VotingService(IEncodingChain encodingChain,
        ITableProvider tableProvider,
        IUserDataProvider userDataProvider)
    {
        _encodingChain = encodingChain;
        _tableProvider = tableProvider;
        _userDataProvider = userDataProvider;
    }

    public void Vote(string userEmail, int idBallot, int candidate)
    {
        var vote = BitConverter.GetBytes(idBallot + candidate);
        _tableProvider.GetEncodingByIdBallot(idBallot).Vote = idBallot + candidate;

        _encodingChain.Encode(new UserVote(idBallot) {EncryptedVote = vote},
            _userDataProvider.GetPrivateUserDataByEmail(userEmail) ?? throw new UserNotFoundException());
    }
}