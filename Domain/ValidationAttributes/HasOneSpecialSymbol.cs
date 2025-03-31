using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.ValidationAttributes;

public class HasOneSpecialSymbol : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            var regex = new Regex(@"(?=.*[@$!%*?&])");
            return regex.IsMatch(str);
        }
        return false;
    }
}
