using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface ICecProvider
{
    public CecData Cec { get; }
}