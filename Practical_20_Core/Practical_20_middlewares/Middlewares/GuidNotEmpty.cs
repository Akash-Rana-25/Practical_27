using System.ComponentModel.DataAnnotations;

namespace Practical_20_middlewares.Middlewares;

public class GuidNotEmpty : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is Guid guidValue)
        {
            return guidValue != Guid.Empty;
        }
        return false;
    }
}
