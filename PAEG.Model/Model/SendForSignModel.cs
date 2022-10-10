using System.ComponentModel.DataAnnotations;

namespace PAEG.Model.Model;

public class SendForSignModel
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public List<List<MaskedBallot>>? Ballots { get; set; }
    
    [Required]
    public int? MaskKey { get; set; }
}