using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryTableProvider: ITableProvider
{
    private static List<EncodingTable> _encodingData = new ();
    private static List<DecodingTable> _decodingData = new ();

    public List<EncodingTable> GetAllEncodingTables()
    {
        return _encodingData;
    }
    public List<DecodingTable> GetAllDecodingTables()
    {
        return _decodingData;
    }

    public EncodingTable GetEncodingByIdBallot(int idBallot)
    {
        var table = _encodingData.FirstOrDefault(t => t.IdBallot == idBallot);
        if (table is null)
        {
            table = new EncodingTable(idBallot);
            _encodingData.Add(table);
        }

        return table;
    }

    public DecodingTable GetDecodingByIdBallot(int idBallot)
    {
        var table = _decodingData.FirstOrDefault(t => t.IdBallot == idBallot);
        if (table is null)
        {
            table = new DecodingTable(idBallot);
            _decodingData.Add(table);
        }

        return table;
    }
}