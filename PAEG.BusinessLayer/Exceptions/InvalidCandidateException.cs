namespace PAEG.BusinessLayer.Exceptions; 

public class InvalidCandidateException: BusinessException {
    public override string ToString()
    {
        return "InvalidCandidate";
    }
}