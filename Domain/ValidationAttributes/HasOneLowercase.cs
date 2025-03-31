using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.ValidationAttributes;

public class HasOneLowercase : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            var regex = new Regex(@"(?=.*[A-Z])");
            if (!regex.IsMatch(str))
            {
                return false;
            }
            return regex.IsMatch(str);
        }
        return false;
    }
}
