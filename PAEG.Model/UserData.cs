using System.Security.Cryptography;

namespace PAEG.Model;

public record UserData(string Email, byte[] Modulus, byte[] Exponent);

public record UserPrivateData(string Email,
    byte[] Modulus,
    byte[] Exponent,
    byte[] D,
    int IdBallot,
    RSAParameters RsaParameters) :
    UserData(Email, Modulus, Exponent);