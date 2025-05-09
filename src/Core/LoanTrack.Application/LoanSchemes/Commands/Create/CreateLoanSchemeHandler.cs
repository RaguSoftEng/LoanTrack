using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.Data;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.LoanSchemes;

namespace LoanTrack.Application.LoanSchemes.Commands.Create;

public class CreateLoanSchemeHandler(
    ILoanSchemeRepository repository,
    IUnitOfWork unitOfWork
) : ICommandHandler<CreateLoanSchemeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateLoanSchemeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var isExists = await repository.IsExistAsync(
                x => x.Name == request.Name,
                cancellationToken
            );

            if (isExists)
                return Result.Failure<Guid>(Error.Failure("500", $"There is already a scheme with name: {request.Name}"));
            
            var code = await repository.GenerateNextSchemeCodeAsync(cancellationToken);

            var scheme = LoanScheme.Create(
                code,
                request.Name,
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

            await repository.AddAsync(scheme, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return scheme.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<Guid>(Error.Failure("500", "There was an error creating the scheme."));
        }
    }
}
