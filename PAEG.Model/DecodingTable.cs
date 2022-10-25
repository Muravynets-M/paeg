namespace PAEG.Model;

public class DecodingTable
{
    public DecodingTable(string idBallot)
    {
        IdBallot = idBallot;
    }

    public string IdBallot { get; }
    
    public bool SignVerified { get; set; }
    public byte[] DecryptedHash { get; set; }
    public int Vote { get; set; }
    
    public string? Exception { get; set; }
}