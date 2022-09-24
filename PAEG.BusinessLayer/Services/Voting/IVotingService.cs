namespace PAEG.BusinessLayer.Services.Voting;

public interface IVotingService
{
    void Vote(int idBallot, int candidate);
}