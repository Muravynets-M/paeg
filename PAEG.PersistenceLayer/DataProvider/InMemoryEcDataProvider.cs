using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryEcDataProvide : IEcProvider
{
    private const int EcsCount = 2;
    private List<(EcData, List<EncryptedBallot>)> _ecs;

    public InMemoryEcDataProvide()
    {
        _ecs = Enumerable.Range(0, EcsCount)
            .Select(i => (new EcData(i), new List<EncryptedBallot>()))
            .ToList();
    }

    public List<EcData> GetAllEcs()
    {
        return _ecs.Select(tuple => tuple.Item1)
            .ToList();
    }

    public void SaveBallot(int idEc, EncryptedBallot ballot)
    {
        var ec = _ecs.FirstOrDefault(tuple => tuple.Item1.Id == idEc);
        ec.Item2.Add(ballot);
    }

    public bool CheckUserHasNotVoted(int idEc, int idUser)
    {
        return !_ecs.Exists(tuple => tuple.Item1.Id == idEc);
    }

    public List<EncryptedBallot> GetAllUserBallots(int idUser)
    {
        return _ecs
            .SelectMany(tuple => tuple.Item2)
            .Where(b => b.IdUser == idUser)
            .ToList();
    }
}