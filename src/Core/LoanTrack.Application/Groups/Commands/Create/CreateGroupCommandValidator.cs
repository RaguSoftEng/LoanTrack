using FluentValidation;

namespace LoanTrack.Application.Groups.Commands.Create;

public class CreateGroupCommandValidator: AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(150);
        RuleFor(c => c.Description).MaximumLength(300);
    }
    
}
