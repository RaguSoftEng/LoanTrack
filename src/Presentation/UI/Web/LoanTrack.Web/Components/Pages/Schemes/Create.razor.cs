using Blazored.Toast.Services;
using LoanTrack.Application.LoanSchemes.Commands.Create;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.LoanSchemes;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Schemes;

public partial class Create(
    ISender sender,
    AppSettingState settingState,
    IToastService toastService,
    NavigationManager navManager
) : ComponentBase
{
    private LoanSchemeVm Entity { get; set; } = new();
    private string NewBorrowerType { get; set; } = string.Empty;
    private string NewLoanPurpose { get; set; } = string.Empty;

    protected override void OnInitialized()
    {
        settingState.CurrentPageName ="New Loan Scheme";
    }

    private async Task OnSubmit()
    {
        var request = new CreateLoanSchemeCommand(
            Entity.Name,
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
            navManager.NavigateTo($"{Paths.SchemesView}/{response.Value}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
    private void OnClear() => Entity = new LoanSchemeVm();
    
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
}

