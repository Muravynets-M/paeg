namespace PAEG.Model; 

public class EncryptedBallot {
    public int Id { get; set; }
    public byte[] Bytes { get; set; }
    public System.Numerics.BigInteger X0 { get; set; }
}