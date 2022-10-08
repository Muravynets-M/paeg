namespace PAEG.Model;

public class DecodingTable
{
    public DecodingTable(int idBallot)
    {
        IdBallot = idBallot;
    }

    public int IdBallot { get; }
    
    public bool SignVerified { get; set; }
    public byte[] DecryptedHash { get; set; }
    public byte[] Gamma { get; set; }
    public byte[] Bytes { get; set; }
    public int Vote { get; set; }
    
    public string? Exception { get; set; }
}