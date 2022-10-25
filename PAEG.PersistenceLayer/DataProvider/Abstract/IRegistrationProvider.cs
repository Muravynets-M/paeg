namespace PAEG.PersistenceLayer.DataProvider.Abstract; 

public interface IRegistrationProvider {
    public bool Exists(string email);
    public void Save(string email, string ballot);
    public bool IsRegistered(string ballot);

    public List<(string, string)> GetAll();
}