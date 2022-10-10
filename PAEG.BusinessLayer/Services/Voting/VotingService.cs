using PAEG.BusinessLayer.Exceptions;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Security.Cryptography;
using System.Text;

namespace PAEG.BusinessLayer.Services.Voting;

public class VotingService : IVotingService {
    private const string SignSecret = "Patron the Dog";

    private readonly int _candidateCount;
    private readonly IVotingCentreDataProvider _votingCentreProvider;
    private readonly ITableProvider _tableProvider;
    public VotingService(int candidateCount, IVotingCentreDataProvider votingCentreProvider, ITableProvider tableProvider)
    {
        _candidateCount = candidateCount;
        _votingCentreProvider = votingCentreProvider;
        _tableProvider = tableProvider;

    }

    public List<SignedMaskedBallot> SendToVotingCenter(List<List<MaskedBallot>> maskedBallots, int maskKey, string email)
    {
        var rand = new Random();
        var ignoredPack = rand.Next(0, maskedBallots.Count);

        for (var i = 0; i < maskedBallots.Count; i++)
        {
            if (i == ignoredPack)
                continue;

            for (var j = 0; j < maskedBallots[i].Count; j++)
            {
                var candidate = BitConverter.ToInt32(IVotingService.Unmask(maskedBallots[i][j].Secret, maskKey));
                var g = new Guid(IVotingService.Unmask(maskedBallots[i][j].Id, maskKey));

                if (!ValidateCandidate(candidate))
                {
                    throw new InvalidCandidateException() {Id = g};
                }
                if (_votingCentreProvider.HasBallotBeenUsed(g))
                {
                    throw new BallotAlreadyUsedException() {Id = g};
                }
                
                _votingCentreProvider.SaveProcessedBallot(g);
            }
        }

        var votingPublicKey = _votingCentreProvider.VotingCentre;
        _votingCentreProvider.SaveUserVoting(email);
        
        using var rsa = RSACryptoServiceProvider.Create();
        rsa.ImportParameters(votingPublicKey.RsaParameters);

        var sign = rsa.SignData(
            Encoding.UTF8.GetBytes(SignSecret),
            HashAlgorithmName.SHA512,
            RSASignaturePadding.Pkcs1
        );

        return maskedBallots[ignoredPack].Select(b => {
            var signedMasked = new SignedMaskedBallot(b.Id, sign, b.Secret);
            var encodingTable = new EncodingTable()
            {
                Guid = new Guid(IVotingService.Unmask(b.Id, maskKey)),
                SignedMaskedBallot = signedMasked
            };
            _tableProvider.SaveEncodingTable(encodingTable);

            return signedMasked;
        }).ToList();

    }
    public void Vote(SignedEncodedBallot ballot)
    {
        var encodingTable = _tableProvider.GetEncodingByIdBallot(ballot.Id);
        encodingTable.SignedEncodedBallot = ballot;

        using var rsa = RSACryptoServiceProvider.Create();
        rsa.ImportParameters(_votingCentreProvider.VotingCentre.RsaParameters);

        if (_votingCentreProvider.HasUserVoted(ballot.Email))
        {
            throw new UserAlreadyVotedException() {Id = ballot.Id};
        }

        if (!rsa.VerifyData(
                Encoding.UTF8.GetBytes(SignSecret),
                ballot.Sign,
                HashAlgorithmName.SHA512,
                RSASignaturePadding.Pkcs1)
           )
        {
            throw new InvalidSignException() {Id = ballot.Id};
        }

        var decryptedVote = BitConverter.ToInt32(rsa.Decrypt(ballot.EncryptedVote, RSAEncryptionPadding.Pkcs1));
        if (!ValidateCandidate(decryptedVote))
        {
            throw new InvalidCandidateException() {Id = ballot.Id};
        }
        encodingTable.DecodedVote = decryptedVote;
        _votingCentreProvider.SaveUserVoting(ballot.Email, ballot.Id);

        _votingCentreProvider.SaveVoteResult(ballot.Id, decryptedVote);
    }
    private bool ValidateCandidate(int candidate)
    {
        return candidate > 0 && candidate <= _candidateCount;
    }
}