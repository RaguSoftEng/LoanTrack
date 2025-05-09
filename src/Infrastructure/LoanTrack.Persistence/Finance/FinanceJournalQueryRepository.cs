using LoanTrack.Application.Finance;
using LoanTrack.Application.Finance.Reports.FinancialPosition;
using LoanTrack.Application.Finance.Reports.GetFinanceSummary;
using LoanTrack.Application.Finance.Reports.LoanSummary;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Finance;

public class FinanceJournalQueryRepository(ApplicationDbContext context) : IFinanceJournalQueryRepository
{
    public async Task<Result<FinanceSummaryResponse>> GetFinanceSummaryAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default
    )
    {
        var result = await context.FinanceJournals.AsNoTracking()
            .Where(x => x.JournalDate >= startDate && x.JournalDate <= endDate)
            .GroupBy(x => x.JournalType)
            .Select(g => new
            {
                g.Key,
                Total = g.Sum(x => x.Amount)
            })
            .ToDictionaryAsync(x => x.Key, x => x.Total, cancellationToken);

        var totalCollection = result
            .Where(kvp => JournalTypes.CollectionTypes.Contains(kvp.Key))
            .Sum(kvp => kvp.Value);

        var totalProfit = result
            .Where(kvp => JournalTypes.ProfitTypes.Contains(kvp.Key))
            .Sum(kvp => kvp.Value);

        return new FinanceSummaryResponse
        {
            TotalCollection = totalCollection,
            TotalProfit = totalProfit,
            TotalInsurance = Get(JournalTypes.Insurance),
            TotalLoanIssued = Get(JournalTypes.LoanIssued),
            TotalProcessFee = Get(JournalTypes.ProcessingFee),
            TotalCapitalGain = Get(JournalTypes.LoanRepayment),
            TotalInterestIncome = Get(JournalTypes.InterestIncome),
            TotalPenalty = Get(JournalTypes.PenaltyIncome)
        };

        double Get(string key) => result.GetValueOrDefault(key, 0);
    }

    public async Task<Result<IReadOnlyList<LoanSummaryResponse>>> GetLoanSummaryAsync(
        Guid centerId,
        Guid groupId,
        string nic,
        bool includeClosed,
        CancellationToken cancellationToken = default
    )
    {

        var result = await (
                from a in context.Loans.AsNoTracking()
                    .Where(x => x.LoanStatus == LoanStatuses.Ongoing
                                || includeClosed && x.LoanStatus == LoanStatuses.Closed
                    )
                    .WhereIf(!string.IsNullOrEmpty(nic), x => x.Customer.Nic == nic)
                    .WhereIf(centerId != Guid.Empty, x => x.Customer.CenterId == centerId)
                    .WhereIf(groupId != Guid.Empty, x => x.Customer.GroupId == groupId)
                join b in context.FinanceJournals.AsNoTracking() on a.Id equals b.ReferenceId into fj
                from d in fj.DefaultIfEmpty()
                group d by new
                {
                    a.Id, a.LoanNumber, a.Customer.FullName, a.Customer.Nic, a.LoanStatus, a.TotalAmountPayable,
                    a.LoanAmount
                }
                into g
                select LoanSummaryResponse.Create(
                    g.Key.Id,
                    g.Key.LoanNumber,
                    $"{g.Key.FullName} | {g.Key.Nic}",
                    g.Key.LoanAmount,
                    g.Key.TotalAmountPayable,
                    g.Where(j => j != null && j.JournalType == "LoanRepayment")
                        .Sum(j => (double?)j.Amount) ?? 0,
                    g.Where(j => j != null && j.JournalType == "InterestIncome")
                        .Sum(j => (double?)j.Amount) ?? 0,
                    g.Where(j => j != null && j.JournalType == "PenaltyIncome")
                        .Sum(j => (double?)j.Amount) ?? 0,
                    g.Where(j => j != null && j.JournalType == "ProcessingFee")
                        .Sum(j => (double?)j.Amount) ?? 0,
                    g.Where(j => j != null && j.JournalType == "Insurance")
                        .Sum(j => (double?)j.Amount) ?? 0,
                    g.Key.LoanStatus
                )
            )
            .ToListAsync(cancellationToken);

        return result;
    }

    /*public async Task<Result<FinancialPositionResponse>> GetFinancialPositionAsync(DateOnly asOfDate, CancellationToken cancellationToken = default)
    {
        var loans=  from a in context.Loans
            .Where(x => x.LoanStatus == LoanStatuses.Ongoing || x.ClosedDate > asOfDate)
            join b in context.FinanceJournals on a.Id equals b.ReferenceId
            select


        var profit = await context.FinanceJournals.AsNoTracking()
            .Where(x => JournalTypes.ProfitTypes.Contains(x.JournalType))
            .SumAsync(x => x.Amount, cancellationToken);

        var insurance = await
    }*/
}
