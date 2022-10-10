using Microsoft.AspNetCore.Mvc;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.Api.Controllers.v1.Table;

[Route("api/v1/table")]
[ApiController]
public class TableController
{
    private ITableProvider _tableProvider;

    public TableController(ITableProvider tableProvider)
    {
        _tableProvider = tableProvider;
    }

    [HttpGet("encoding")]
    public IEnumerable<EncodingTable> GetEncodingTables()
    {
        return _tableProvider.GetAllEncodingTables();
    }
}