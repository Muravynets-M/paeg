using System.Security.Cryptography;

namespace PAEG.Model;

public record UserPrivateData(string Email, string Identitification, DSAParameters DsaParameters);