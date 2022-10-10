using Microsoft.AspNetCore.Mvc;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.Api.Controllers.v1.UserData;

[Route("api/v1/user-data")]
[ApiController]
public class UserDataController
{
    private readonly IUserDataProvider _userDataProvider;

    public UserDataController(IUserDataProvider userDataProvider)
    {
        _userDataProvider = userDataProvider;
    }

    [HttpGet("get-all")]
    public IEnumerable<Model.UserData> GetUserData([FromQuery(Name = "private")]bool? sendPrivate)
    {
        if (sendPrivate.HasValue && sendPrivate.Value)
        {
            return _userDataProvider.GetUserData();
        }

        return _userDataProvider.GetUserData();
    }
}