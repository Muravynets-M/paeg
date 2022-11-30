using ElGamalExt;
using System.Security.Cryptography;

namespace PAEG.Model;

public record UserPrivateData(int Id, RSAParameters RsaParameters);