using FluentValidation;

namespace EMS.Application.Features.Department.Commands.Delete;

public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().ForEach(v => v.GreaterThan(0));
    }
}


