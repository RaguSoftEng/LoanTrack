using LoanTrack.Application.LoanSchemes.Queries.GetById;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.LoanSchemes;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Schemes;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    private LoanSchemeVm Entity { get; set; } = new();
    [Parameter] public string Id { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Loan Scheme";
        await LoadLoanScheme();
    }

    private async Task LoadLoanScheme()
    {
        var schemeId = Guid.Parse(Id);
        var response = await sender.Send(new GetLoanSchemeByIdQuery(schemeId));
        if (response.IsSuccess)
        {
            var loanScheme = response.Value;
            Entity = new LoanSchemeVm
            {
                Id = loanScheme.Id,
                Code = loanScheme.Code,
                Description = loanScheme.Description,
                Name = loanScheme.Name,
                CollateralType = loanScheme.CollateralType,
                InterestType = loanScheme.InterestType,
                InterestRate = loanScheme.InterestRate,
                IsActive = loanScheme.IsActive,
                MaximumAmount = loanScheme.MaximumAmount,
                MinimumAmount = loanScheme.MinimumAmount,
                InsuranceAmount = loanScheme.InsuranceAmount,
                RequiresGuarantor = loanScheme.RequiresGuarantor,
                AllowedLoanPurposes = loanScheme.AllowedLoanPurposes,
                EligibleBorrowerTypes = loanScheme.EligibleBorrowerTypes,
                IsGovernmentSubsidized = loanScheme.IsGovernmentSubsidized,
                IsSecuredLoan = loanScheme.IsSecuredLoan,
                LatePaymentPenalty = loanScheme.LatePaymentPenalty,
                ProcessingFee = loanScheme.ProcessingFee,
                GracePeriodInMonths = loanScheme.GracePeriodInMonths,
                HasFixedInterestRate = loanScheme.HasFixedInterestRate,
                RepaymentPeriodsInMonths = loanScheme.RepaymentPeriodsInMonths
            };
            StateHasChanged();
        }
    }  
}

