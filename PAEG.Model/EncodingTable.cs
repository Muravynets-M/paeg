namespace PAEG.Model;

public class EncodingTable
{
    public Guid Guid { get; set; }

    public SignedMaskedBallot SignedMaskedBallot { get; set; }
    
    public SignedEncodedBallot SignedEncodedBallot { get; set; }
    
    public int DecodedVote { get; set; }
    
    public string? Error { get; set; }
}