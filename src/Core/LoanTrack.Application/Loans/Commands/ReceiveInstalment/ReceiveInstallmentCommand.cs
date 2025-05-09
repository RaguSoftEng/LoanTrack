using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Commands.ReceiveInstalment;

public record ReceiveInstallmentCommand(
    Guid InstalmentId,
    double Amount,
    DateOnly PaymentDate,
    bool IsDelayed,
    int DaysDelayed,
    string PaymentMethod,
    bool IsPenaltyApplied,
    double PenaltyAmount,
    string PenaltyReason,
    string PaymentDescription
): ICommand;
