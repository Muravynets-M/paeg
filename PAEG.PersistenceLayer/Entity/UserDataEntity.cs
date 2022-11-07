using ElGamalExt;
using System.Security.Cryptography;

namespace PAEG.PersistenceLayer.Entity;

public record UserDataEntity(int  Id, RSAParameters RsaParametersShort, RSAParameters RsaParametersLong, ElGamalParameters ElGamalParameters);