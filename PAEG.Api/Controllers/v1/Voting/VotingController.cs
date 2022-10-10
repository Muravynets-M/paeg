using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PAEG.BusinessLayer.Exceptions;
using PAEG.BusinessLayer.Services.Calculation;
using PAEG.BusinessLayer.Services.Voting;
using PAEG.Model;
using PAEG.Model.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Security.Cryptography;

namespace PAEG.Api.Controllers.v1.Voting;

[Route("api/v1/voting")]
[ApiController]
public class VotingController {
    private readonly IVotingService _votingService;
    private readonly ICalculationService _calculationService;
    private readonly ITableProvider _tableProvider;
    private IVotingCentreDataProvider _votingCentre;

    public VotingController(IVotingService votingService, ICalculationService calculationService, ITableProvider tableProvider, IVotingCentreDataProvider votingCentre)
    {
        _votingService = votingService;
        _calculationService = calculationService;
        _tableProvider = tableProvider;
        _votingCentre = votingCentre;
    }

    [HttpPost("send-for-sign")]
    public ActionResult<List<SignedMaskedBallot>> SendForSign(SendForSignModel sendForSign)
    {
        try
        {
            return _votingService.SendToVotingCenter(sendForSign.Ballots!, (int) sendForSign.MaskKey!, sendForSign.Email);

        }
        catch (BusinessException exception)
        {
            _tableProvider.SaveEncodingTable(new EncodingTable() {Guid = exception.Id, Error = exception.ToString()});

            return new BadRequestObjectResult(exception.ToString());
        }
    }

    [HttpPost]
    public IActionResult Vote(SignedEncodedBallot signedEncodedBallot)
    {
        try
        {
            using var rsa = RSACryptoServiceProvider.Create();
            rsa.ImportParameters(_votingCentre.VotingCentre.RsaParameters);

            _votingService.Vote(signedEncodedBallot with
            {
                EncryptedVote = rsa.Encrypt(signedEncodedBallot.EncryptedVote, RSAEncryptionPadding.Pkcs1)
            });

            return new OkResult();
        }
        catch (BusinessException exception)
        {
            _tableProvider.SaveEncodingTable(new EncodingTable() {Guid = exception.Id, Error = exception.ToString()});

            return new BadRequestObjectResult(exception.ToString());
        }
    }

    [HttpPost("close")]
    public void CloseVoting()
    {
        _calculationService.CalculateVotes();
    }

    [HttpGet("results")]
    public IEnumerable<VoteResult> GetResults()
    {
        return _calculationService.GetVoteResults();
    }
}