using FluentValidation;

namespace LoanTrack.Application.Employees.Commands.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(command => command.LastName)
            .MaximumLength(100)
            .NotEmpty();
        RuleFor(command => command.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(command => command.Password)
            .MaximumLength(25)
            .MinimumLength(8)
            .NotEmpty();
    }
}
