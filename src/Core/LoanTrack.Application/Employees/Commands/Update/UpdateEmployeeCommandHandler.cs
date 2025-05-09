using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Application.Common.Identity;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Commands.Update;

public class UpdateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IIdentityProviderService identity,
    IUnitOfWork unitOfWork
): ICommandHandler<UpdateEmployeeCommand>
{
    public async Task<Result> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (employee is null) return Result.Failure(EmployeeErrors.NotFound(request.Id));
            
            if (employee.Role != request.UserRole)
            {
                await identity.UpdateUserRoleAsync(employee.IdentityId, request.UserRole, cancellationToken);
            }

            if (employee.FirstName == request.FirstName && employee.LastName == request.LastName)
            {
                return Result.Success();
            }

            employee.UpdateProfile(request.FirstName, request.LastName);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(Error.Failure("500", "Unable to update the profile."));
        }
    }
}
