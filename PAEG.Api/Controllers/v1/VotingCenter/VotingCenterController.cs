using Microsoft.AspNetCore.Mvc;
using PAEG.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;
using System.Security.Cryptography;

namespace PAEG.Api.Controllers.v1; 

[Route("api/v1/voting-center")]
[ApiController]
public class VotingCenterController {
    private IVotingCentreDataProvider _votingCentreProvider;

    public VotingCenterController(IVotingCentreDataProvider votingCentreProvider)
    {
        _votingCentreProvider = votingCentreProvider;
    }

    [HttpGet]
    public RSAParameters GetRsaParams()
    {
        return _votingCentreProvider.VotingCentre.RsaParameters;
    }
}