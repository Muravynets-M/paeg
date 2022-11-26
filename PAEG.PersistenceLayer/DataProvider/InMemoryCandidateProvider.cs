using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryCandidateProvider: ICandidateProvider
{
    private List<CandidateData> _candidates = new();
    
    public List<CandidateData> GetAllCandidates()
    {
        return _candidates.ToList();
    }
}