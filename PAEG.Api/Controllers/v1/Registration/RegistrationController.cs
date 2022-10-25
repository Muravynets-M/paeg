using Microsoft.AspNetCore.Mvc;
using PAEG.BusinessLayer.Exceptions;
using PAEG.BusinessLayer.Services.RegistrationService;
using PAEG.Model.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.Api.Controllers.v1.Registration;

[Route("api/v1/registration")]
[ApiController]
public class RegistrationController {
    private readonly IRegistrationService _registrationService;
    private readonly IRegistrationProvider _registrationProvider;

    public RegistrationController(IRegistrationService registrationService, IRegistrationProvider registrationProvider)
    {
        _registrationService = registrationService;
        _registrationProvider = registrationProvider;
    }

    [HttpPost]
    public IActionResult Register(RegistrationModel model)
    {
        try
        {
            return new OkObjectResult(_registrationService.Register(model.Email!));
        }
        catch (BusinessException exception)
        {
            return new BadRequestObjectResult(exception.ToString());
        }
    }

    [HttpGet("list")]
    public List<RegistrationListModel> List()
    {
        return _registrationProvider.GetAll()
            .Select(pair => new RegistrationListModel{Email = pair.Item1, Ballot = pair.Item2})
            .ToList();
    }
}