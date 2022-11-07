namespace PAEG.PersistenceLayer.DataProvider.Abstract; 

public interface IVoterRandomStringsProvider {
    public void Save(int idVoter, int order, byte[] randomString);
    public byte[] GetByIdAndOrder(int idVoter, int order);
}