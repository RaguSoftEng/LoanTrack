using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Customers.Commands.UpdateCustomer;

public record UpdateCustomerCommand(
    Guid CustomerId,
    string Nic,
    string FullName,
    string Gender,
    string Email,
    string PhoneNumber,
    string Address,
    Guid DsDivision,
    Guid District,
    Guid Province,
    Guid GramaNiladhari,
    Guid Center,
    Guid? Group,
    string Occupation,
    DateOnly DateOfBirth,
    string? BankName,
    string? BankBranch,
    string? BankAccountNumber,
    string? AccountName,
    string? WorkAddress
) : ICommand;
