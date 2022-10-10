using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface ITableProvider
{
    public List<EncodingTable> GetAllEncodingTables();

    public void SaveEncodingTable(EncodingTable encodingTable);
    public EncodingTable GetEncodingByIdBallot(Guid guid);
}