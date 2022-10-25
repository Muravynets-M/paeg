namespace PAEG.Model;

public class UserVote
{
    public string Ballot { get; }
    
    public string Identification { get; set; }
    public byte[] Sign { get; set; }
    public byte[] EncryptedVote { get; set; }

    public UserVote(string ballot)
    {
        Ballot = ballot;
    }
}