using System.Security.Cryptography;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.PersistenceLayer.DataProvider;

public class InMemoryCecProvider: ICecProvider
{
    public InMemoryCecProvider()
    {
        using var rsa = RSACryptoServiceProvider.Create();
        Cec = new CecData(rsa.ExportParameters(true));
    }

    public CecData Cec { get; }
}