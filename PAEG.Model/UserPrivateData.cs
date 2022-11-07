using ElGamalExt;
using System.Security.Cryptography;

namespace PAEG.Model;

public record UserPrivateData(int Id, RSAParameters RsaParametersShort, RSAParameters RsaParametersLong, ElGamalParameters ElGamalParameters);