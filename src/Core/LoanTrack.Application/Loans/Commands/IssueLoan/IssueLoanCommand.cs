using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Commands.IssueLoan;

public record IssueLoanCommand(Guid LoanId, DateOnly IssueDate, DateOnly FistInstallmentDate) : ICommand;
