using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(ICustomerRepository repository, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCustomerCommand>
{
    public async Task<Result> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Customer? customer = await repository.GetByIdAsync(request.CustomerId, cancellationToken);

            if (customer is null)
            {
                return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
            }
            
            var isExists = await repository.IsExistAsync(
                x => x.Nic == request.Nic && x.Id != customer.Id,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"There is already a customer with NIC: {request.Nic}"));

            customer.Update(
                request.Nic,
                request.FullName,
                request.Gender,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.GramaNiladhari == Guid.Empty ? null : request.GramaNiladhari,
                request.DsDivision == Guid.Empty ? null : request.DsDivision,
                request.District == Guid.Empty ? null : request.District,
                request.Province == Guid.Empty ? null : request.Province,
                request.Center == Guid.Empty ? null : request.Center,
                request.Group  == Guid.Empty ? null : request.Group,
                request.Occupation,
                request.WorkAddress,
                request.DateOfBirth,
                request.BankName,
                request.BankBranch,
                request.BankAccountNumber,
                request.AccountName
            );

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Failure(Error.Failure("500", "Unable to update customer."));
        }
    }
}
