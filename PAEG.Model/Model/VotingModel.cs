using System.ComponentModel.DataAnnotations;

namespace PAEG.Model.Model;

public class VoteModel
{
    [Required]
    public int IdBallot { get; set; }

    [Required]
    public int Candidate { get; set; }

    public override string ToString()
    {
        return $"{IdBallot}: {Candidate}";
    }
}