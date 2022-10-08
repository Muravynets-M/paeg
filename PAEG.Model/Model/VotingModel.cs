using System.ComponentModel.DataAnnotations;

namespace PAEG.Model.Model;

public class VoteModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public int? IdBallot { get; set; }

    [Required]
    public int? Candidate { get; set; }

    public override string ToString()
    {
        return $"{Email}: {IdBallot} + {Candidate}";
    }
}