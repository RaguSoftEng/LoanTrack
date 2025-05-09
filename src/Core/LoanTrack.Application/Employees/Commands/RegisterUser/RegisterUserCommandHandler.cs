using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Application.Common.Identity;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Commands.RegisterUser;

public class RegisterUserCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    IIdentityProviderService identityProviderService
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Result<string> result = await identityProviderService.RegisterUserAsync(
                new UserModel(
                    request.Email,
                    request.Password,
                    request.FirstName,
                    request.LastName,
                    request.UserRole
                ),
                cancellationToken
            );
        
            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }
        
            var user = Employee.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                result.Value,
                request.UserRole
            );
        
            await employeeRepository.AddAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result.Failure<Guid>(Error.Failure("500", ex.Message));
        }
    }
}
