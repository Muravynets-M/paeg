using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IEcProvider
{
    public List<EcData> GetAllEcs();
    public void SaveBallot(int idEc, EncryptedBallot ballot);
    public bool CheckUserHasVoted(int idEc, int idUser);
    public List<EncryptedBallot> GetAllUserBallots(int idUser);
}