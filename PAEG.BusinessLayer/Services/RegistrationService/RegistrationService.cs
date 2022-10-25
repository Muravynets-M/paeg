using PAEG.BusinessLayer.Exceptions;
using PAEG.Model.Model;
using PAEG.PersistenceLayer.DataProvider.Abstract;

namespace PAEG.BusinessLayer.Services.RegistrationService; 

public class RegistrationService: IRegistrationService {
    private IRegistrationProvider _registrationProvider;
    
    public RegistrationService(IRegistrationProvider registrationProvider)
    {
        _registrationProvider = registrationProvider;
    }
    
    public RegistrationResultModel Register(string email)
    {
        if (_registrationProvider.Exists(email))
            throw new EmailAlreadyRegisteredException();

        var ballot = $"#{Faker.Identification.SocialSecurityNumber()}";
        _registrationProvider.Save(email, ballot);

        return new RegistrationResultModel(ballot);
    }
}