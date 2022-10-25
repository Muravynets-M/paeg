using System.Security.Cryptography;

namespace PAEG.PersistenceLayer.Entity;

public record UserDataEntity(string Email, string Identity,  DSAParameters RsaParameters);