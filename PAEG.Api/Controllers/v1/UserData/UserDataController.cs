using Microsoft.AspNetCore.Mvc;
using PAEG.PersistenceLayer.DataProvider;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.Api.Controllers.v1.UserData;

[Route("api/v1/user-data")]
[ApiController]
public class UserDataController {
    private readonly IUserProvider _userProvider;

    public UserDataController(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    [HttpGet("get-all")]
    public IEnumerable<Model.UserPrivateData> GetUserData([FromQuery(Name = "private")] bool? sendPrivate)
    {
        var users = _userProvider.GetPrivateUserData();

        return users;
    }
}