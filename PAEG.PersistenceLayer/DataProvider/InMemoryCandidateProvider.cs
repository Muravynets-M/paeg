using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryCandidateProvider : ICandidateProvider
{
    private List<CandidateData> _candidates = new();
    private const int CandidateCount = 2;

    public InMemoryCandidateProvider()
    {
        var r = new Random();
        _candidates = Enumerable.Range(0, CandidateCount)
            .Select(i => new CandidateData(r.Next(2, 5) * r.Next(3, 6)))
            .ToList();
    }

    public List<CandidateData> GetAllCandidates()
    {
        return _candidates.ToList();
    }
}