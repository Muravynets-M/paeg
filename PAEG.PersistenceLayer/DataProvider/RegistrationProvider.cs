using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider; 

public class RegistrationProvider: IRegistrationProvider {
    private static List<(string, string)> _identities = new();

    public bool Exists(string email)
    {
        return _identities.Exists(identity => identity.Item1 == email);
    }
    
    public void Save(string email, string ballot)
    {
        _identities.Add((email, ballot));
    }

    public bool IsRegistered(string ballot)
    {
        return _identities.Exists(identity => identity.Item2 == ballot);
    }
    
    public List<(string, string)> GetAll()
    {
        return _identities;
    }
}