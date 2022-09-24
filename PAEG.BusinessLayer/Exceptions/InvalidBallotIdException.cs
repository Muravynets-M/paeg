namespace PAEG.BusinessLayer.Exceptions; 

public class InvalidBallotIdException: BusinessException {
    public override string ToString()
    {
        return "InvalidBallotId";
    }
}