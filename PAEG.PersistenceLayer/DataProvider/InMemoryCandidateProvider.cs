using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryCandidateProvider : ICandidateProvider
{
    private List<CandidateData> _candidates = new();
    private const int CandidateCount = 2;

    public InMemoryCandidateProvider()
    {
        _candidates = Enumerable.Range(1, 2).Select(i => new CandidateData(i, Faker.Name.FullName())).ToList();
    }

    public List<CandidateData> GetAllCandidates()
    {
        return _candidates.ToList();
    }
}