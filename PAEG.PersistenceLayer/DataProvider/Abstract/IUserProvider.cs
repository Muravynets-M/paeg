using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IUserProvider
{
    IEnumerable<UserPrivateData> GetPrivateUserData();
    UserPrivateData? GetPrivateUserDataByEmail(string email);
}