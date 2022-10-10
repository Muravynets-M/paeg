namespace PAEG.BusinessLayer.Exceptions; 

public class UserAlreadyVotedException: BusinessException {
    public override string ToString()
    {
        return "UserAlreadyVoted";
    }
}