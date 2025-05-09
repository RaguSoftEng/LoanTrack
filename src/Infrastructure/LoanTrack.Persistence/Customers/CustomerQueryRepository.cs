using System.Globalization;
using System.Linq.Expressions;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Queries;
using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Domain.Customers;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Customers;

public class CustomerQueryRepository(ApplicationDbContext context) : ICustomerQueryRepository
{


    public async Task<CustomerResponse?> GetByNicAsync(string nic, CancellationToken cancellationToken = default)
        => await context.Customers.Where(x => x.Nic.ToLower() == nic.ToLower())
            .Select(ProjectCustomerResponseExpr).AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<CustomerResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var results = await context.Customers.Where(x => x.Id == id)
                .Select(ProjectCustomerResponseExpr)
                .FirstOrDefaultAsync(cancellationToken);
            return results;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<PaginatedResult<CustomerResponse>> GetCustomersByFilter(
        QueryParameters queryParams,
        Guid centerId,
        Guid groupId,
        CancellationToken cancellationToken = default
    )
    {
        var query = context.Customers
            .AsNoTracking()
            .WhereIf(centerId != Guid.Empty, x => x.CenterId == centerId)
            .WhereIf(groupId != Guid.Empty, x => x.GroupId == groupId)
            .SmartSearch(queryParams.SearchBy, queryParams.Search);

        var count = await query.CountAsync(cancellationToken);

        var results = await query
            .SmartSort(queryParams.SortBy, queryParams.SortDescending)
            .SmartPaging(queryParams.Page, queryParams.PageSize)
            .Select(ProjectCustomerResponseExpr)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CustomerResponse>
        {
            Items = results,
            TotalCount = count,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }

    public async Task<Guid?> GetCustomerIdByNicAsync(string nic, CancellationToken cancellationToken = default)
        => await context.Customers.Where(x => x.Nic.ToLower() == nic.ToLower())
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<(string Center, string Group, int Count)>> GetCustomersCountsByCenter(
        CancellationToken cancellationToken = default
    )
    {
        var results = await context.Customers.AsNoTracking()
            .GroupBy(x => new
            {
                x.CenterId,
                CenterName = x.Center != null ? x.Center.Name : "Unknown Center",
                x.GroupId,
                GroupName = x.Group != null ? x.Group.Name : "Unknown Group"
            })
            .Select(g => new
            {
                g.Key.CenterName,
                g.Key.GroupName,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        return results.Select(x => (x.CenterName, x.GroupName, x.Count)).ToList();
    }
    
    private static Expression<Func<Customer, CustomerResponse>> ProjectCustomerResponseExpr =>
        x => new CustomerResponse(
            x.Id,
            x.Code.ToString(CultureInfo.CurrentCulture),
            x.Nic,
            x.FullName,
            x.Gender,
            x.Email,
            x.PhoneNumber,
            x.Address,
            x.GramaNiladhariId ?? Guid.Empty,
            x.GramaNiladhari != null ? x.GramaNiladhari.Description : string.Empty,
            x.DsDivisionId ?? Guid.Empty,
            x.DsDivision != null ? x.DsDivision.Description : string.Empty,
            x.DistrictId ?? Guid.Empty,
            x.District != null ? x.District.Description : string.Empty,
            x.ProvinceId ?? Guid.Empty,
            x.Province != null ? x.Province.Description : string.Empty,
            x.CenterId ?? Guid.Empty,
            x.Center != null ? x.Center.Name : string.Empty,
            x.GroupId,
            x.Group != null ? x.Group.Name : "",
            x.Occupation ?? "",
            x.DateOfBirth,
            x.BankName ?? "",
            x.BankBranch ?? "",
            x.BankAccountNumber ?? "",
            x.AccountName ?? "",
            x.WorkAddress ?? ""
        );
}
