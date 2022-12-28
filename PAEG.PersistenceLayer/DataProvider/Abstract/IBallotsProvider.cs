using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IBallotsProvider
{
    public List<EncryptedBallot> GetAllBallots();
    public bool Exists(int id);
    public void Save(EncryptedBallot ballot);
}