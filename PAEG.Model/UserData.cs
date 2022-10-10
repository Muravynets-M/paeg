using PAEG.Model.Model;
using System.Security.Cryptography;

namespace PAEG.Model;

public record UserData(string Email, RSAParameters RsaParameters, IEnumerable<IEnumerable<Ballot>> BallotPacks);
