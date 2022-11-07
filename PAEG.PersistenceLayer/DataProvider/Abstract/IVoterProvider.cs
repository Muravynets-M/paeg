using PAEG.Model;

namespace PAEG.PersistenceLayer.DataProvider.Abstract;

public interface IVoterProvider
{
    IEnumerable<UserPrivateData> GetPrivateUserData();
    UserPrivateData? GetPrivateUserDataById(int id);
}