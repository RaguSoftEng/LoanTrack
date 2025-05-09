using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Commands.ReceiveBulkInstallment;

public record ReceiveBulkInstallmentCommand(
    List<(Guid InstalmentId, double PaidAmount, DateOnly PaymentDate, bool IsDelayed, int DaysDelayed)> Installments
):ICommand;
