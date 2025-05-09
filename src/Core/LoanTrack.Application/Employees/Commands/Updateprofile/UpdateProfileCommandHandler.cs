using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Employees;

namespace LoanTrack.Application.Employees.Commands.Updateprofile;

public class UpdateProfileCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateProfileCommand>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);
            if (employee is null) return Result.Failure(EmployeeErrors.NotFound(request.Id));

            employee.UpdateProfile(request.FirstName, request.LastName);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(Error.Failure("500", "Unable to update your profile."));
        }
    }
}
