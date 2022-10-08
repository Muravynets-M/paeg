using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IUserDataProvider
{
    IEnumerable<UserPrivateData> GetPrivateUserData();
    IEnumerable<UserData> GetPublicUserData();
    UserPrivateData? GetPrivateUserDataByEmail(string email);
}