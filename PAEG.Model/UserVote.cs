namespace PAEG.Model;

public class UserVote
{
    public int IdBallot { get; }
    public byte[] Sign { get; set; }
    public byte[] EncryptedVote { get; set; }

    public UserVote(int idBallot)
    {
        IdBallot = idBallot;
    }
}