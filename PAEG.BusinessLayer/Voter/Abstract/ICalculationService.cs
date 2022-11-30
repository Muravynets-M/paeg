namespace PAEG.BusinessLayer.Voter;

public interface ICalculationService
{
    public List<(int, int)> CalculateVotes();
}