namespace PAEG.Model; 

public record Ballot(int Candidate, Guid Id);

public record MaskedBallot(byte[] Id, byte[] Secret);

public record SignedMaskedBallot(byte[] Id, byte[] Sign, byte[] Secret);

public record SignedEncodedBallot(Guid Id, byte[] Sign, byte[] EncryptedVote, string Email);
