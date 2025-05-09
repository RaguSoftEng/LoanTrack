using LoanTrack.Application.Loans;
using LoanTrack.Application.Loans.Queries.Installments;
using LoanTrack.Application.Loans.Queries.Reports.GetCollection;
using LoanTrack.Domain.Common;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Loans;

public class InstallmentQueryRepository(ApplicationDbContext context) : IInstallmentQueryRepository
{
    public async Task<IReadOnlyCollection<InstallmentsListResponse>> GetInstallmentsByLoanAsync(Guid loanId,
        CancellationToken cancellationToken = default)
        => await context.LoanInstallments.AsNoTracking()
            .Where(x => x.LoanId == loanId)
            .OrderBy(x => x.InstallmentDate)
            .Select(x => new InstallmentsListResponse(
                x.LoanId,
                x.Id,
                x.Loan.LoanNumber,
                $"{x.Loan.Customer.FullName} | {x.Loan.Customer.Nic}",
                x.InstallmentNumber,
                x.InstallmentDate,
                x.InstallmentAmount,
                x.IsPaid,
                x.PaymentDate,
                x.AmountPaid,
                IsDelayed(x.InstallmentDate, x.IsPaid, x.IsDelayed),
                DaysDelayed(x.InstallmentDate, x.IsPaid, x.DelayedDays),
                x.Loan.TotalAmountPayable - x.Loan.PaidAmount,
                GetInstallmentStatus(x.IsPaid, x.InstallmentDate, x.PaymentDate)
            ))
            .ToListAsync(cancellationToken);

    public async Task<GetInstallmentResponse> GetInstallmentByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
        => await context.LoanInstallments.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new GetInstallmentResponse(
                x.LoanId,
                x.Id,
                x.Loan.LoanNumber,
                x.InstallmentNumber,
                x.InstallmentDate,
                x.InstallmentAmount,
                x.IsPaid,
                IsDelayed(x.InstallmentDate, x.IsPaid, x.IsDelayed),
                DaysDelayed(x.InstallmentDate, x.IsPaid, x.DelayedDays),
                x.IsPenaltyApplied,
                x.PenaltyAmount,
                x.PenaltyReason,
                x.PaymentDate,
                x.AmountPaid,
                x.PaymentMethod,
                x.PaymentDescription,
                GetInstallmentStatus(x.IsPaid, x.InstallmentDate, x.PaymentDate)
            )).FirstOrDefaultAsync(cancellationToken);

    public async Task<GetInstallmentResponse?> GetNextInstallmentByLoanAsync(Guid loanId,
        CancellationToken cancellationToken = default)
        => await context.LoanInstallments.AsNoTracking()
            .Where(x => x.LoanId == loanId && !x.IsPaid)
            .OrderBy(x => x.InstallmentNumber)
            .Select(x => new GetInstallmentResponse(
                x.LoanId,
                x.Id,
                x.Loan.LoanNumber,
                x.InstallmentNumber,
                x.InstallmentDate,
                x.InstallmentAmount,
                x.IsPaid,
                IsDelayed(x.InstallmentDate, x.IsPaid, x.IsDelayed),
                DaysDelayed(x.InstallmentDate, x.IsPaid, x.DelayedDays),
                x.IsPenaltyApplied,
                x.PenaltyAmount,
                x.PenaltyReason,
                x.PaymentDate,
                x.AmountPaid,
                x.PaymentMethod,
                x.PaymentDescription,
                GetInstallmentStatus(x.IsPaid, x.InstallmentDate, x.PaymentDate)
            )).FirstOrDefaultAsync(cancellationToken);

    public async Task<List<InstallmentsListResponse>> GetNextInstallmentsByGroupAndCenterAsync(
        Guid centerId,
        Guid groupId,
        bool includeHistory,
        CancellationToken cancellationToken = default
    )
        => await context.LoanInstallments.AsNoTracking()
            .WhereIf(groupId != Guid.Empty, x => x.Loan.Customer.GroupId == groupId)
            .WhereIf(
                centerId != Guid.Empty && groupId == Guid.Empty,
                x => x.Loan.Customer.CenterId == centerId
            )
            .WhereIf(!includeHistory, x => !x.IsPaid)
            .OrderBy(x => x.InstallmentDate)
            .Select(x => new InstallmentsListResponse(
                x.LoanId,
                x.Id,
                x.Loan.LoanNumber,
                $"{x.Loan.Customer.FullName} | {x.Loan.Customer.Nic}",
                x.InstallmentNumber,
                x.InstallmentDate,
                x.InstallmentAmount,
                x.IsPaid,
                x.PaymentDate,
                x.AmountPaid,
                IsDelayed(x.InstallmentDate, x.IsPaid, x.IsDelayed),
                DaysDelayed(x.InstallmentDate, x.IsPaid, x.DelayedDays),
                x.Loan.TotalAmountPayable - x.Loan.PaidAmount,
                GetInstallmentStatus(x.IsPaid, x.InstallmentDate, x.PaymentDate)
            ))
            .ToListAsync(cancellationToken);

    public async Task<List<InstallmentsListResponse>> GetNextInstallmentsByCustomerAsync(
        Guid customerId,
        bool includeHistory,
        CancellationToken cancellationToken = default
    )
        => await context.LoanInstallments.AsNoTracking()
            .Where(x => x.Loan.CustomerId == customerId)
            .WhereIf(!includeHistory, x => !x.IsPaid)
            .OrderBy(x => x.InstallmentDate)
            .Select(x => new InstallmentsListResponse(
                x.LoanId,
                x.Id,
                x.Loan.LoanNumber,
                $"{x.Loan.Customer.FullName} | {x.Loan.Customer.Nic}",
                x.InstallmentNumber,
                x.InstallmentDate,
                x.InstallmentAmount,
                x.IsPaid,
                x.PaymentDate,
                x.AmountPaid,
                IsDelayed(x.InstallmentDate, x.IsPaid, x.IsDelayed),
                DaysDelayed(x.InstallmentDate, x.IsPaid, x.DelayedDays),
                x.Loan.TotalAmountPayable - x.Loan.PaidAmount,
                GetInstallmentStatus(x.IsPaid, x.InstallmentDate, x.PaymentDate)
            ))
            .ToListAsync(cancellationToken);

    public Task<int> GetInstallmentsCountForTodayAsync(CancellationToken cancellationToken = default)
        => context.LoanInstallments.AsNoTracking()
            .Where(x => !x.IsPaid && x.InstallmentDate == DateOnly.FromDateTime(DateTime.Now))
            .CountAsync(cancellationToken);

    public Task<int> GetOverDueInstallmentsCountAsync(CancellationToken cancellationToken = default)
        => context.LoanInstallments.AsNoTracking()
            .Where(x => !x.IsPaid && x.InstallmentDate < DateOnly.FromDateTime(DateTime.Now))
            .CountAsync(cancellationToken);

    public async Task<Result<CollectionResponse>> GetCollectionAsync(
        DateOnly dateFrom,
        DateOnly dateTo,
        CancellationToken cancellationToken = default
    )
    {
        var result = await context.LoanInstallments
            .AsNoTracking()
            .Where(x => x.IsPaid && x.PaymentDate >= dateFrom && x.PaymentDate <= dateTo)
            .GroupBy(x => new
            {
                Center = x.Loan.Customer.Center != null ? x.Loan.Customer.Center.Name : "Unknown Center",
                Group = x.Loan.Customer.Group != null ? x.Loan.Customer.Group.Name : "Unknown Group"
            })
            .Select(g => new
            {
                g.Key.Center,
                g.Key.Group,
                Collection = g.Sum(x => x.AmountPaid)
            })
            .ToListAsync(cancellationToken);

        return new CollectionResponse
        {
            StartDate = dateFrom,
            EndDate = dateTo,
            GroupCollection = [.. result.Select(x => (x.Center, x.Group, x.Collection))],
            CenterCollection =
            [
                .. result
                    .GroupBy(x => x.Center)
                    .Select(g => (Center: g.Key, Collection: g.Sum(x => x.Collection)))
            ],
            TotalCollection = result.Sum(x => x.Collection)
        };
    }

    #region Helper methods
    private static string GetInstallmentStatus(bool isPaid, DateOnly installmentDate, DateOnly? paymentDate)
        => isPaid switch
        {
            true when paymentDate >= installmentDate => "Delayed Payment",
            true when paymentDate <= installmentDate => "Paid",
            false when installmentDate < DateOnly.FromDateTime(DateTime.UtcNow)
                => $"Overdue for {DaysDelayed(installmentDate, false, 0)} days.",
            _ => "Pending"
        };


    private static bool IsDelayed(DateOnly installmentDate, bool isPaid, bool isDelayed)
        => !isPaid ? installmentDate.DayNumber < DateOnly.FromDateTime(DateTime.UtcNow.Date).DayNumber : isDelayed;

    private static int DaysDelayed(DateOnly installmentDate, bool isPaid, int days)
        => isPaid
            ? days
            : DateOnly.FromDateTime(DateTime.UtcNow.Date).DayNumber - installmentDate.DayNumber > 0
                ? DateOnly.FromDateTime(DateTime.UtcNow.Date).DayNumber - installmentDate.DayNumber
                : 0;
    #endregion

}
