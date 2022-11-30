using System.Security.Cryptography;
using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Voter;

public class EcSendingService : IEcSendingService
{
    private readonly IEcProvider _ecProvider;
    private readonly IVoterProvider _voterProvider;

    public EcSendingService(IEcProvider ecProvider, IVoterProvider voterProvider)
    {
        _ecProvider = ecProvider;
        _voterProvider = voterProvider;
    }

    public void SendToEc(int idEc, SignedBallot ballot)
    {
        if (_ecProvider.CheckUserHasVoted(idEc, ballot.IdUser))
        {
            throw new BallotAlreadyUsedException();
        }

        var userData = _voterProvider.GetPrivateUserDataById(ballot.IdUser);
        using var dsa = RSACryptoServiceProvider.Create();
        dsa.ImportParameters(userData.RsaParameters);
        if (!dsa.VerifyData(
                BitConverter.GetBytes(ballot.IdUser),
                ballot.Sign, 
                HashAlgorithmName.SHA1, 
                RSASignaturePadding.Pkcs1))
        {
            throw new InvalidSignException();
        }

        _ecProvider.SaveBallot(idEc, ballot);
    }
}