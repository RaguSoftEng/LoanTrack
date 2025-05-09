using System.Globalization;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Loans;
using LoanTrack.Application.Loans.Queries.GetLoanById;
using LoanTrack.Application.Loans.Queries.GetLoanCustomer;
using LoanTrack.Application.Loans.Queries.GetLoans;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Loans;

public class LoanQueryRepository(ApplicationDbContext context) : ILoanQueryRepository
{
    public async Task<GetLoanCustomerResponse?> GetLoanCustomerInfoAsync(string nic,
        CancellationToken cancellationToken = default)
    {
        nic = nic.ToLower(CultureInfo.CurrentCulture);
        var customer = await context.Customers.Where(c => c.Nic.ToLower() == nic)
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                CustomerInfo = $"Name:{x.FullName}\nContact No: {x.PhoneNumber}\nAddress: {x.Address}",
                x.Code
            }).FirstOrDefaultAsync(cancellationToken);

        if (customer == null) return null;

        string lastCode = await context.Loans
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(x => x.CustomerId == customer.Id)
            .OrderByDescending(x => x.LoanNumber)
            .Select(x => x.LoanNumber)
            .FirstOrDefaultAsync(cancellationToken);

        int nextNumber = 1;
        if (!string.IsNullOrEmpty(lastCode))
        {
            string[] parts = lastCode.Split('-');

            if (parts.Length == 3 && int.TryParse(parts[2], out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        var datePart = DateTime.UtcNow.ToString("yyyyMM", CultureInfo.CurrentCulture);

        return new GetLoanCustomerResponse(
            customer.Id,
            customer.CustomerInfo,
            $"LN{datePart}-{customer.Code}-{nextNumber}"
        );
    }

    public async Task<PaginatedResult<GetLoansResponse>> GetLoansByFilterAsync(
        QueryParameters parameters,
        Guid centerId,
        Guid groupId,
        string nic,
        CancellationToken cancellationToken = default
    )
    {
        var query = context.Loans
            .AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(nic), x => x.Customer.Nic == nic)
            .WhereIf(centerId != Guid.Empty, x => x.Customer.CenterId == centerId)
            .WhereIf(groupId != Guid.Empty, x => x.Customer.GroupId == groupId)
            .SmartSearch(parameters.SearchBy, parameters.Search);

        var count = await query.CountAsync(cancellationToken);

        var results = await query
            .SmartSort(parameters.SortBy, parameters.SortDescending)
            .SmartPaging(parameters.Page, parameters.PageSize)
            .Select(x => new GetLoansResponse(
                x.Id,
                x.LoanNumber,
                $"{x.Customer.FullName} | {x.Customer.Nic}",
                x.LoanAmount,
                x.TotalAmountPayable - x.PaidAmount,
                x.IssuanceDate,
                x.EndDate,
                x.LoanStatus
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<GetLoansResponse>
        {
            Items = results,
            TotalCount = count,
            Page = parameters.Page,
            PageSize = parameters.PageSize
        };
    }

    public async Task<GetLoanByIdResponse?> GetLoanByIdAsync(Guid loanId, CancellationToken cancellationToken = default)
        => await context.Loans.AsNoTracking()
            .Where(x => x.Id == loanId)
            .Select(x => new GetLoanByIdResponse(
                x.Id,
                x.LoanNumber,
                $"{x.Customer.FullName} | {x.Customer.Nic}",
                x.LoanScheme != null ? x.LoanScheme.Code : string.Empty,
                x.LoanSchemeId,
                x.LoanOfficer,
                x.LoanAmount,
                x.InterestType,
                x.InterestRate,
                x.InstallmentType,
                x.DurationInInterestUnits,
                x.RepaymentDurations,
                x.InstallmentAmount,
                x.IssuanceDate,
                x.EndDate,
                x.NextInstallmentDate,
                x.LoanDisbursementMethod,
                x.LoanRepaymentMethod,
                x.GuarantorsInformation,
                x.LoanStatus,
                x.ClosedDate,
                x.TotalAmountPayable,
                x.PaidAmount,
                x.ProcessingFee,
                x.InsuranceAmount
            ))
            .FirstOrDefaultAsync(cancellationToken);


    public async Task<IReadOnlyList<(string Status, int Count)>> GetLoanCountsByStatusAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await context.Loans
            .AsNoTracking()
            .GroupBy(x => x.LoanStatus)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToListAsync(cancellationToken);

        return [.. result.Select(x => (x.Status, x.Count))];
    }
}
