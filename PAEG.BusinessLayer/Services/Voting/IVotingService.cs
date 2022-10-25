namespace PAEG.BusinessLayer.Services.Voting;

public interface IVotingService
{
    void Vote(string email, string identification, string ballot, int candidate);
}