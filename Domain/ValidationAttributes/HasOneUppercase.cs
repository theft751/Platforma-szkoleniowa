using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.ValidationAttributes;

public class HasOneUppercase : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            var regex = new Regex(@"(?=.*[A-Z])");
            return regex.IsMatch(str);
        }
        return false;
    }
}
