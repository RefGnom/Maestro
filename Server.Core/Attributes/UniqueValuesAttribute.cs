using System.ComponentModel.DataAnnotations;

namespace Maestro.Server.Core.Attributes;

public class UniqueValuesAttribute() : ValidationAttribute("List values should be unique")
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IList<long> list)
            return new ValidationResult("Value should be convertible to List<long>");

        return list.Count != list.Distinct().Count() ? new ValidationResult("List values is not unique") : ValidationResult.Success;
    }
}