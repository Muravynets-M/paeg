using System.ComponentModel.DataAnnotations;

namespace PAEG.Model.Model;

public class VoteModel
{
    [EmailAddress]
    public string? Email { get; set; }
    
    [Required]
    public string? Identification { get; set; }
    
    [Required]
    public string? Ballot { get; set; }

    [Required]
    public int? Candidate { get; set; }

    public override string ToString()
    {
        return $"{Identification}: {Ballot} + {Candidate}";
    }
}