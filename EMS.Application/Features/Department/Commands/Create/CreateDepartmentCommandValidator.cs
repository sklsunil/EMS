using FluentValidation;

namespace EMS.Application.Features.Department.Commands.Create;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(v => v.Name)
             .MaximumLength(256)
             .NotEmpty();
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
 {
     var result = await ValidateAsync(ValidationContext<CreateDepartmentCommand>.CreateWithOptions((CreateDepartmentCommand)model, x => x.IncludeProperties(propertyName)));
     if (result.IsValid)
         return Array.Empty<string>();
     return result.Errors.Select(e => e.ErrorMessage);
 };
}

