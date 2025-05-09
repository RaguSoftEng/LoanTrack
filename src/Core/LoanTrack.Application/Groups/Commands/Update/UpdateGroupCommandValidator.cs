using FluentValidation;

namespace LoanTrack.Application.Groups.Commands.Update;

public class UpdateGroupCommandValidator :AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(150);
        RuleFor(c => c.Description).MaximumLength(300);
    }
}
