namespace PAEG.BusinessLayer.Exceptions; 

public class BallotAlreadyUsedException: BusinessException {
    public override string ToString()
    {
        return "BallotAlreadyUsed";
    }
}