namespace PAEG.BusinessLayer.Services.Voting;

public interface IVotingService
{
    void Vote(string userEmail, int idBallot, int candidate);
}