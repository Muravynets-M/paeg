namespace PAEG.Model;

public class EncodingTable
{
    public EncodingTable(string ballot)
    {
        Ballot = ballot;
    }

    public string Ballot { get; }
    public string Identification { get; set; }
    public int Vote { get; set; }
    
    public byte[]? Bytes { get; set; }
    public byte[]? ElGamal { get; set; }
    public byte[]? SignedHash { get; set; }
}