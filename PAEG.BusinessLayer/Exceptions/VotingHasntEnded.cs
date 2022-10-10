namespace PAEG.BusinessLayer.Exceptions; 

public class VotingHasntEnded: BusinessException
{
    public override string ToString()
    {
        return "VotingHasntEnded";
    }
}