using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface ICandidateProvider
{
    public List<CandidateData> GetAllCandidates();
}