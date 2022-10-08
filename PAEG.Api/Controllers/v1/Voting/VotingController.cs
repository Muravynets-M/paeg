using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PAEG.BusinessLayer.Exceptions;
using PAEG.BusinessLayer.Services.Calculation;
using PAEG.BusinessLayer.Services.Voting;
using PAEG.Model;
using PAEG.Model.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.Api.Controllers.v1.Voting;

[Route("api/v1/voting")]
[ApiController]
public class VotingController {
    private readonly IVotingService _votingService;
    private readonly ICalculationService _calculationService;
    private readonly IVotingCentreDataProvider _votingCentreDataProvider;

    public VotingController(IVotingService votingService, ICalculationService calculationService, IVotingCentreDataProvider votingCentreDataProvider)
    {
        _votingService = votingService;
        _calculationService = calculationService;
        _votingCentreDataProvider = votingCentreDataProvider;
    }

    [HttpPost]
    public IActionResult Vote(VoteModel voteModel)
    {
        try
        {
            _votingService.Vote(voteModel.Email ?? "", voteModel.IdBallot ?? 0, voteModel.Candidate ?? 0);

            return new OkResult();
        }
        catch (BusinessException exception)
        {
            return new NotFoundResult();
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
        return _votingCentreDataProvider.GetVoteResults();
    }
}