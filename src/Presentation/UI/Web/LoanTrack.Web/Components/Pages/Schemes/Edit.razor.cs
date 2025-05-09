using Blazored.Toast.Services;
using LoanTrack.Application.LoanSchemes.Commands.Update;
using LoanTrack.Application.LoanSchemes.Queries.GetById;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.LoanSchemes;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Schemes;

public partial class Edit(
    ISender sender,
    AppSettingState appSettingState,
    IToastService toastService,
    NavigationManager navManager
) : ComponentBase
{
    private LoanSchemeVm Entity { get; set; } = new();
    [Parameter] public string Id { get; set; }
    private string NewBorrowerType { get; set; } = string.Empty;
    private string NewLoanPurpose { get; set; } = string.Empty;
    
    
    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Loan Scheme";
        await GetLoadLoanSchemeAsync();
    }
    
    private void AddBorrowerType()
    {
        if (string.IsNullOrWhiteSpace(NewBorrowerType))
        {
            return;
        }

        Entity.EligibleBorrowerTypes.Add(NewBorrowerType);
        NewBorrowerType = string.Empty;
    }

    private void AddLoanPurpose()
    {
        if (string.IsNullOrWhiteSpace(NewLoanPurpose))
        {
            return;
        }

        Entity.AllowedLoanPurposes.Add(NewLoanPurpose);
        NewLoanPurpose = string.Empty;
    }
    
    private async Task GetLoadLoanSchemeAsync()
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
    
    private async Task OnSubmit()
    {
        var request = new UpdateLoanSchemeCommand(
            Entity.Id,
            Entity.Description,
            Entity.InterestType,
            Entity.InterestRate,
            Entity.MaximumAmount,
            Entity.MaximumAmount,
            Entity.RepaymentPeriodsInMonths,
            Entity.ProcessingFee,
            Entity.InsuranceAmount,
            Entity.LatePaymentPenalty,
            Entity.IsSecuredLoan,
            Entity.CollateralType,
            Entity.HasFixedInterestRate,
            Entity.IsGovernmentSubsidized,
            Entity.EligibleBorrowerTypes,
            Entity.AllowedLoanPurposes,
            Entity.RequiresGuarantor,
            Entity.GracePeriodInMonths,
            Entity.IsActive
        );
        
        var response = await sender.Send(request);
        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Your scheme has been created.");
            navManager.NavigateTo($"{Paths.SchemesView}/{Entity.Id}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

