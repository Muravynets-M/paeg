using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

class InMemoryBallotsProvider : IBallotsProvider
{
    private static readonly List<EncryptedBallot> _ballots = new List<EncryptedBallot>();
    
    public List<EncryptedBallot> GetAllBallots()
    {
        return _ballots;
    }

    public bool Exists(int id)
    {
        return _ballots.Exists(b => b.Id == id);
    }

    public void Save(EncryptedBallot ballot)
    {
        _ballots.Add(ballot);
    }
}