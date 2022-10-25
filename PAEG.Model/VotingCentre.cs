using System.Security.Cryptography;

namespace PAEG.Model;

public record VotingCentre(byte[] Modulus, byte[] Exponent);
public record PrivateVotingCentre(DSAParameters RsaParameters);