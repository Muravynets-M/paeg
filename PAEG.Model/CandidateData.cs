using System.Security.Cryptography;

namespace PAEG.Model;

public record CandidateData(int Id, RSAParameters RsaParameters);