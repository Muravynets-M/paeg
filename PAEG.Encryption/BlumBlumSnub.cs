using System.Numerics;

namespace PAEG.Encryption;

public class BlumBlumSnub
{
    private static readonly BigInteger P = 3263849;
    private static readonly BigInteger Q = 1302498943;
    public static readonly BigInteger N = P * Q;
    public static readonly List<BigInteger> Seeds = new List<BigInteger>(){2, 3, 5, 7, 11};

    public static BigInteger Next(BigInteger prev)
    {
        return (prev * prev) % N;
    }

    public static bool Lsb(BigInteger n)
    {
        return (n & BigInteger.One) != BigInteger.Zero;
    }
}