using FluentValidation;

namespace EMS.Application.Features.Employee.Commands.AddEdit;

public class AddEditEmployeeCommandValidator : AbstractValidator<AddEditEmployeeCommand>
{
    public AddEditEmployeeCommandValidator()
    {
        RuleFor(v => v.Name)
            .MaximumLength(256)
            .NotEmpty();

        RuleFor(v => v.Email)
                 .MaximumLength(256)
                 .NotEmpty();

        RuleFor(v => v.DOB)
                .NotEmpty();
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<AddEditEmployeeCommand>.CreateWithOptions((AddEditEmployeeCommand)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}

