
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class HasOneNumber : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            var regex = new Regex(@"(?=.*\d)");
            return regex.IsMatch(str);
        }
        return false;
    }
}
