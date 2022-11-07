using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider; 

public class VoterRandomStringsProvider: IVoterRandomStringsProvider {
    private readonly IDictionary<int, IDictionary<int, byte[]>> _randomStrings = new Dictionary<int, IDictionary<int, byte[]>>();

    public void Save(int idVoter, int order, byte[] randomString)
    {
        if (!_randomStrings.ContainsKey(idVoter))
        {
            _randomStrings.Add(idVoter, new Dictionary<int, byte[]>());
        }
        
        _randomStrings[idVoter].Add(order, randomString);
    }
    
    public byte[] GetByIdAndOrder(int idVoter, int order)
    {
        return _randomStrings[idVoter][order];
    }
}