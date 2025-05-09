using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Customers.Queries;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Application.Loans.Queries.Installments.GetInstallmentsByCustomer;

public class GetInstallmentsByCustomerNicQueryHandler(
    IInstallmentQueryRepository repository,
    ICustomerQueryRepository customerRepository
): IQueryHandler<GetInstallmentsByCustomerNicQuery, IReadOnlyCollection<InstallmentsListResponse>>
{
    public async Task<Result<IReadOnlyCollection<InstallmentsListResponse>>> Handle(GetInstallmentsByCustomerNicQuery request,
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetCustomerIdByNicAsync(request.Nic, cancellationToken);
        if (customer == null)
            return Result.Failure<IReadOnlyCollection<InstallmentsListResponse>>(CustomerErrors.NotFound(request.Nic));
        
        return await repository.GetNextInstallmentsByCustomerAsync(customer.Value, false, cancellationToken);
    }
}
