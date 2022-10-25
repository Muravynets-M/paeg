using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface ITableProvider
{
    public List<EncodingTable> GetAllEncodingTables();
    public List<DecodingTable> GetAllDecodingTables();
    public EncodingTable GetEncodingByIdBallot(string idBallot);
    public DecodingTable GetDecodingByIdBallot(string idBallot);
}