using PAEG.Model.Model;

namespace PAEG.BusinessLayer.Services.RegistrationService; 

public interface IRegistrationService {
    public RegistrationResultModel Register(string email);
}