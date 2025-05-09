using LoanTrack.Application.Loans.Queries.Installments;
using LoanTrack.Web.Shared.Common;

namespace LoanTrack.Web.Shared.Loans;

public sealed class InstallmentVm
{
    public Guid LoanId { get; set; }
    public Guid InstallmentId { get; set; }
    public string LoanNumber { get; set; }
    public string Customer  { get; set; }
    public int InstallmentNumber { get; set; }
    public DateOnly InstallmentDate { get; set; }
    public DateOnly PaymentDate { get; set; }
    public double InstallmentAmount { get; set; }
    public double PaymentAmount { get; set; }
    public string Status { get; set; }
    public bool IsDelayed { get; set; }
    public int DaysDelayed { get; set; }
    public double LoanBalance { get; set; }
    public bool IsPaid { get; set; }

    public static InstallmentVm LoadByInstallmentResponse(InstallmentsListResponse response)
        => new()
        {
            LoanId = response.LoanId,
            InstallmentId = response.InstallmentId,
            LoanNumber = response.LoanNumber,
            Customer = response.Customer,
            InstallmentNumber = response.InstallmentNumber,
            InstallmentDate = response.InstallmentDate,
            InstallmentAmount = response.InstallmentAmount,
            Status = response.Status,
            PaymentDate = DateOnly.FromDateTime(DateTime.UtcNow),
            PaymentAmount = response.InstallmentAmount,
            IsDelayed = response.IsDelayed,
            DaysDelayed = response.DelayedDays,
            LoanBalance = response.LoanBalance,
            IsPaid = false,
        };
}
