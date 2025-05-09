using System.Text.RegularExpressions;
using FluentValidation;

namespace LoanTrack.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(c => c.FullName).NotEmpty();
        RuleFor(c => c.DateOfBirth).NotEmpty();
        RuleFor(c => c.Nic).Must((cus, _) => IsValidSriLankanNic(cus.Nic));
        RuleFor(c=>c.Address).NotEmpty();
    }
    
    private static bool IsValidSriLankanNic(string nic)
    {
        if (string.IsNullOrWhiteSpace(nic))
            return false;

        // Validate Old NIC Format (9 Digits + V or X)
        if (Regex.IsMatch(nic, @"^\d{9}[VXvx]$"))
        {
            var birthYear = int.Parse($"19{nic.Substring(0, 2)}");
            return IsValidYear(birthYear);
        }

        // Validate New NIC Format (12 Digits)
        if (Regex.IsMatch(nic, @"^\d{12}$"))
        {
            var birthYear = int.Parse(nic.Substring(0, 4));
            return IsValidYear(birthYear);
        }
        return false;
    }

    private static bool IsValidYear(int birthYear)
        => birthYear >= 1900 && birthYear <= DateTime.Now.Year;
}
