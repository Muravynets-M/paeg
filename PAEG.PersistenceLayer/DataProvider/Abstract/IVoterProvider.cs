using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IVoterProvider
{
    IEnumerable<UserPrivateData> GetRegisteredPrivateUserData();
    public UserPrivateData GetUnregisteredPrivateUserData();
    UserPrivateData? GetPrivateUserDataById(int id);
    public UserPrivateData? GetPrivateUserDataByLoginPassword(string login, string password);
}