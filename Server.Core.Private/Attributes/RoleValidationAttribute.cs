using System.ComponentModel.DataAnnotations;
using Maestro.Data.Core;
using Maestro.Server.Private.Services;

namespace Maestro.Server.Private.Attributes;

public class RoleValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string role)
        {
            return new ValidationResult("Role should be a string");
        }

        if (validationContext.GetService(typeof(IRolesValidator)) is not IRolesValidator rolesValidator)
        {
            throw new InvalidOperationException($"Unable to resolve {nameof(IRolesValidator)} for {nameof(RoleValidationAttribute)}.");
        }
        
        return rolesValidator.Validate(role) ? ValidationResult.Success : new ValidationResult("Role is not valid");
    }
}