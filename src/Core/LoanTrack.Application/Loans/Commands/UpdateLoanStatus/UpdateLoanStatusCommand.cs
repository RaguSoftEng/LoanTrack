using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Commands.UpdateLoanStatus;

public record UpdateLoanStatusCommand(Guid LoanId, string Status, DateOnly? ActionDate = null ) : ICommand;
