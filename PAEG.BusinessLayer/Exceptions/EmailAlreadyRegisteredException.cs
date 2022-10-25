namespace PAEG.BusinessLayer.Exceptions;

public class EmailAlreadyRegisteredException : BusinessException {

    public override string ToString()
    {
        return "EmailAlreadyRegistered";
    }
}