using System.Security.Cryptography;

namespace PAEG.PersistenceLayer.Entity;

public record UserDataEntity(string Email, int IdBallot, RSAParameters RsaParameters);