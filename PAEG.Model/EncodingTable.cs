namespace PAEG.Model;

public class EncodingTable
{
    public EncodingTable(int idBallot)
    {
        IdBallot = idBallot;
    }

    public int IdBallot { get; }
    
    public int Vote { get; set; }
    public byte[] Gamma { get; set; }
    public byte[] SignedHash { get; set; }
    public byte[] EncryptedHash { get; set; }
}