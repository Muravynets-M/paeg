using System.ComponentModel.DataAnnotations;

namespace PAEG.Model.Model; 

public class PrepareForSignModel {
    [Required]
    public string? Email { get; set; }
    
    [Required]
    public int? MaskKey { get; set; }
}