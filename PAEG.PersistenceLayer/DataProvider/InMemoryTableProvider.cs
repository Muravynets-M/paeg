using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using PAEG.PersistenceLayer.Entity;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryTableProvider : ITableProvider {
    private static List<(string, EncodingTable)> _encodingData = new();
    private static List<(string, DecodingTable)> _decodingData = new();

    public List<EncodingTable> GetAllEncodingTables()
    {
        return _encodingData.Select(t => t.Item2).ToList();
    }
    public List<DecodingTable> GetAllDecodingTables()
    {
        return _decodingData.Select(t => t.Item2).ToList();
    }

    public EncodingTable GetEncodingByIdBallot(string idBallot)
    {
        var table = _encodingData
            .LastOrDefault(t => t.Item1 == idBallot).Item2;
        if (table == null || table.SignedHash != null)
        {
            table = new EncodingTable(idBallot);
            _encodingData.Add((idBallot, table));
        }

        return table;
    }

    public DecodingTable GetDecodingByIdBallot(string idBallot)
    {
        var table = _decodingData
            .LastOrDefault(t => t.Item1 == idBallot).Item2;
        if (table == null || table.Vote != 0 || table.Exception != null)
        {
            table = new DecodingTable(idBallot);
            _decodingData.Add((idBallot, table));
        }

        return table;
    }
}