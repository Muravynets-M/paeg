namespace PAEG.Model.Model; 

public class RegistrationResultModel {
    public string Ballot { get; }

    public RegistrationResultModel(string ballot)
    {
        Ballot = ballot;
    }
}