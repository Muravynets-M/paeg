using PAEG.Model;

namespace PAEG.BusinessLayer.Voter;

public interface IEcSendingService
{
    public void SendToEc(int idEc, SignedBallot ballot);
}