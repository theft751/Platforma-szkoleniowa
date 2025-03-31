using Domain.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "First name is required")]
    [MinLength(2, ErrorMessage = "Firstname must have minimum of 2 cymbols")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")]
    [MinLength(2, ErrorMessage = "Lastname must have minimum of 2 cymbols")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must have minimum of 8 cymbols")]
    [HasOneNumber(ErrorMessage = "Password must have at list one number")]
    [HasOneLowercase(ErrorMessage = "Password must have at list one lowercase letter")]
    [HasOneUppercase(ErrorMessage = "Password must have at list one uppercase letter")]
    [HasOneSpecialSymbol(ErrorMessage = "Password must have at list one special symbol")]
    public string Password { get; set; } = null!;
    public string Gmina { get; set; } = null!;
}
