using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.LoanSchemes;

namespace LoanTrack.Application.LoanSchemes.Commands.Update;

public class UpdateLoanSchemeHandler(
    ILoanSchemeRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateLoanSchemeCommand>
{
    public async Task<Result> Handle(UpdateLoanSchemeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loanScheme = await repository.GetByIdAsync(request.Id, cancellationToken);
            if (loanScheme == null) return Result.Failure(Error.NotFound("404", "Scheme not found"));

            loanScheme.Update(
                request.Description,
                request.InterestType,
                request.InterestRate,
                request.MinimumAmount,
                request.MaximumAmount,
                request.RepaymentPeriodsInMonths,
                request.ProcessingFee,
                request.InsuranceAmount,
                request.LatePaymentPenalty,
                request.IsSecuredLoan,
                request.CollateralType,
                request.HasFixedInterestRate,
                request.IsGovernmentSubsidized,
                request.EligibleBorrowerTypes,
                request.AllowedLoanPurposes,
                request.RequiresGuarantor,
                request.GracePeriodInMonths,
                request.IsActive
            );
        
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<Guid>(Error.Failure("500", "Unable to update scheme.")); 
        }
    }
}
