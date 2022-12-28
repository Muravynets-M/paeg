using ElGamalExt;
using System.Numerics;

namespace PAEG.Model;

public record UserPrivateData(string Login, string Password, Token Token)
{
    public string? SerialNumber { get; set; }
};


public record Token(int Id, BbsParameters BbsParams, ElGamalManaged ElGamalParams);

public struct BbsParameters
{
    public BbsParameters(System.Numerics.BigInteger n)
    {
        N = n;
    }

    public System.Numerics.BigInteger N { get; }
}