using System.ComponentModel.DataAnnotations;

namespace PAEG.Model.Model;

public class RegistrationModel {
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
}