using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IUserDataProvider
{
    IEnumerable<UserData> GetUserData();
    UserData? GetUserDataByEmail(string email);
}