namespace PAEG.Model; 

public class SignedBallot: EncryptedBallot {
    public byte[] Sign { get; set; }
}