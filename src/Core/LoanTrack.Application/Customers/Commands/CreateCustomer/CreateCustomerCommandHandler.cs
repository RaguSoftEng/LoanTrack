using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Customers;

namespace LoanTrack.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateCustomerCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExists = await repository.IsExistAsync(
                x => x.Nic == request.Nic,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"There is already a customer with NIC: {request.Nic}"));
            
            var customer = Customer.Create(
                request.Nic,
                request.FullName,
                request.Gender,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.GramaNiladhari,
                request.DsDivision,
                request.District,
                request.Province,
                request.Center,
                request.Group,
                request.Occupation,
                request.DateOfBirth,
                request.BankName,
                request.BankBranch,
                request.BankAccountNumber,
                request.AccountName,
                request.WorkAddress
            );

            await repository.AddAsync(customer, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<Guid>(Error.Failure("500", ex.Message));
        }
    }
}
