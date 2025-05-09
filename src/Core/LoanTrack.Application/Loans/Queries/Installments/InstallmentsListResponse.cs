namespace LoanTrack.Application.Loans.Queries.Installments;

public record InstallmentsListResponse(
    Guid LoanId,
    Guid InstallmentId,
    string LoanNumber,
    string Customer,
    int InstallmentNumber,
    DateOnly InstallmentDate,
    double InstallmentAmount,
    bool IsPaid,
    DateOnly? PaymentDate,
    double AmountPaid,
    bool IsDelayed,
    int DelayedDays,
    double LoanBalance,
    string Status
);
