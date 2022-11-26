using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IEcProvider
{
    public List<EcData> GetAllEcs();
    
}