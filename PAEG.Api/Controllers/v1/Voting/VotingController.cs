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
    private readonly IVotingCentreProvider _votingCentreProvider;

    public VotingController(IVotingService votingService, ICalculationService calculationService, IVotingCentreProvider votingCentreProvider)
    {
        _votingService = votingService;
        _calculationService = calculationService;
        _votingCentreProvider = votingCentreProvider;
    }

    [HttpPost]
    public IActionResult Vote(VoteModel voteModel)
    {
        try
        {
            _votingService.Vote(voteModel.Email!, voteModel.Identification!, voteModel.Ballot!, voteModel.Candidate ?? 0);

            return new OkResult();
        }
        catch (BusinessException exception)
        {
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
        return _votingCentreProvider.GetVoteResults();
    }
}