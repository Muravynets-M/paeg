using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryTableProvider : ITableProvider {
    private static List<EncodingTable> _encodingData = new();
    public List<EncodingTable> GetAllEncodingTables()
    {
        return _encodingData;
    }
    
    public void SaveEncodingTable(EncodingTable encodingTable)
    {
        _encodingData.Add((encodingTable));
        GetEncodingByIdBallot(encodingTable.Guid);
    }

    public EncodingTable GetEncodingByIdBallot(Guid guid)
    {
        var table = _encodingData
            .Where(t => t.Guid == guid)
            .FirstOrDefault();

        return table;
    }
}