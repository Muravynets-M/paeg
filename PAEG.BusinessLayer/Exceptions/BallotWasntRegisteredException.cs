namespace PAEG.BusinessLayer.Exceptions; 

public class BallotWasntRegisteredException: BusinessException {
    public override string ToString()
    {
        return "BallotWasntRegistered";
    }
}