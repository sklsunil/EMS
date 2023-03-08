using FluentValidation;

namespace EMS.Application.Features.Employee.Commands.Delete;

public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));
    }
}


