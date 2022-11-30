namespace PAEG.BusinessLayer.Encryption;

public class ManualRsa
{
    private static int N { get; } = 33;
    private static int E { get; } = 7;
    private static int D { get; } = 3;

    public static int Encrypt(int m)
    {
        long x = 1;
        for (var i = 0; i < E; i++)
        {
            x *= m;
        }
        return (int)(x % N);
    }

    public static int Decrypt(int c)
    {
        long x = 1;
        for (var i = 0; i < D; i++)
        {
            x *= c;
        }
        return (int)(x % N);
    }
}