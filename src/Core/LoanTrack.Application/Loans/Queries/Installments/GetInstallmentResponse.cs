namespace LoanTrack.Application.Loans.Queries.Installments;

public record GetInstallmentResponse(
    Guid LoanId,
    Guid InstallmentId,
    string LoanNumber,
    int InstallmentNumber,
    DateOnly InstallmentDate,
    double InstallmentAmount,
    bool IsPaid,
    bool IsDelayed,
    int DelayedDays,
    bool IsPenaltyApplied,
    double PenaltyAmount,
    string PenaltyReason,
    DateOnly? PaymentDate,
    double AmountPaid,
    string PaymentMethod,
    string PaymentDescription,
    string Status
);
