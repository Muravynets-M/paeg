namespace PAEG.BusinessLayer.Exceptions; 

public class InvalidSignException: BusinessException {
    public override string ToString()
    {
        return "InvalidSign";
    }
}