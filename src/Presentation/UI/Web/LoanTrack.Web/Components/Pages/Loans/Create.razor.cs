using System.ComponentModel.DataAnnotations;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Loans.Commands.CreateLoan;
using LoanTrack.Application.Loans.Queries.GetLoanCustomer;
using LoanTrack.Application.LoanSchemes.Queries;
using LoanTrack.Application.LoanSchemes.Queries.GetAsListValues;
using LoanTrack.Application.LoanSchemes.Queries.GetById;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Loans;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Loans;

public partial class Create(
    ISender sender,
    AppSettingState appSetting,
    NavigationManager navigationManager
) : ComponentBase
{
    private LoanCreateModel Entity { get; set; } = new();
    private string SelectedCustomer { get; set; } = string.Empty;
    private string _nic = string.Empty;
    private Guid _selectedScheme  = Guid.Empty;
    private List<string> _interestTypes = [];
    private List<string> _installmentTypes = [];
    private List<string> _disbursementMethods = [];
    private List<string> _repaymentMethods = [];
    private LoanSchemeResponse? SelectedScheme { get; set; }
    private readonly List<ListValueResponse> _schemes = [];
    private bool _isInitialized;

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "New Loan";
        _interestTypes = [.. InterestTypes.GetTypes];
        GetInstallmentTypes(_interestTypes[0]);
        _disbursementMethods = [..LoanDisbursementMethods.GetMethods];
        _repaymentMethods = [..LoanRepaymentMethods.GetMethods];
        await LoadSchemesAsync();
    }
    
    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender || _isInitialized)
        {
            return;
        }

        Entity.InterestType = _interestTypes[0];
        Entity.InstallmentType = _installmentTypes[0];
        Entity.LoanDisbursementMethod = _disbursementMethods[0];
        Entity.LoanRepaymentMethod = _repaymentMethods[0];
        _isInitialized = true;
    }

    private async Task OnSubmit()
    {
        var result = await sender.Send(new CreateLoanCommand(
            Entity.LoanNumber,
            Entity.CustomerId,
            Entity.LoanSchemeId,
            Entity.LoanOfficer,
            Entity.InterestType,
            Entity.LoanAmount,
            Entity.InterestRate,
            Entity.InstallmentType,
            Entity.DurationInInterestUnits,
            Entity.RepaymentDurations,
            Entity.InstallmentAmount,
            Entity.IssuanceDate,
            Entity.FirstInstallmentDate,
            Entity.LoanDisbursementMethod,
            Entity.LoanRepaymentMethod,
            Entity.GuarantorsInformation,
            Entity.TotalPayable,
            Entity.ProcessingFee,
            Entity.InsuranceAmount
        ));
        if (result.IsSuccess)
        {
            ToastService.ShowSuccess("Loan Created Successfully");
            navigationManager.NavigateTo($"{Paths.LoanView}/{result.Value}");
        }
        else
        {
            ToastService.ShowError(result.Error.Description);
        }
    }
    
    private void OnClear() => Entity = new LoanCreateModel();

    private async Task LoadSchemesAsync()
    {
       var response = await sender.Send(new GetSchemesAsListValuesQuery());
       _schemes.Add(new ListValueResponse(Guid.Empty, "Custom", true));
       if (response.IsSuccess)
       {
           _schemes.AddRange([..response.Value]);
       }
       StateHasChanged();
    }

    private async Task OnSchemeChanged(Guid schemeId)
    {
        if (schemeId == Guid.Empty)
        {
            _selectedScheme = schemeId;
            SelectedScheme = null;
        }
        else
        {
            _selectedScheme = schemeId;
            var response = await sender.Send(new GetLoanSchemeByIdQuery(schemeId));
            if (response.IsSuccess)
            {
                SelectedScheme = response.Value;
                Entity.LoanSchemeId = response.Value.Id;
                Entity.InterestRate = response.Value.InterestRate;
                Entity.InterestType = response.Value.InterestType;
                Entity.ProcessingFee = response.Value.ProcessingFee;
                Entity.InsuranceAmount = response.Value.InsuranceAmount;
                GetInstallmentTypes(Entity.InterestType);
            }
        }
    }

    private static string GetBaseUnit(string interestType) => interestType switch
    {
        InterestTypes.PerDay => "Days",
        InterestTypes.PerMonth => "Months",
        InterestTypes.PerWeek => "Weeks",
        InterestTypes.PerAnnum => "Years",
        _ => "Months"
    };

    private void GetInstallmentTypes(string value)
    {
        Entity.InterestType = value;
        _installmentTypes = Entity.InterestType switch
        {
            InterestTypes.PerMonth => [InstallmentTypes.Monthly, InstallmentTypes.Weekly, InstallmentTypes.Daily],
            InterestTypes.PerWeek => [InstallmentTypes.Weekly, InstallmentTypes.Daily],
            InterestTypes.PerAnnum =>
                [InstallmentTypes.Yearly, InstallmentTypes.Monthly, InstallmentTypes.Weekly, InstallmentTypes.Daily],
            InterestTypes.PerDay => [InstallmentTypes.Daily],
            _ => [..InstallmentTypes.GetTypes]
        };
        OnInstallmentTypeChanged(_installmentTypes[0]);
        StateHasChanged();
    }

    private void OnInstallmentTypeChanged(string value)
    {
        Entity.InstallmentType = value;
        Entity.RepaymentDurations = LoanCalculator.CalDuration(Entity.InterestType, value, Entity.DurationInInterestUnits);
        CalculateInstallment();
    }

    private void OnDurationInInterestUnitsChanged(int value)
    {
        Entity.DurationInInterestUnits = value;
        Entity.RepaymentDurations = LoanCalculator.CalDuration(Entity.InterestType, Entity.InstallmentType, value);
        CalculateInstallment();
    }

    private void OnDurationChanged(int value)
    {
        Entity.RepaymentDurations = value;
        CalculateInstallment();
    }

    private void CalculateInstallment()
    {
        bool isValid = Entity.InterestRate is > 0 and < 100
                       && Entity.DurationInInterestUnits is > 0
                       && Entity.RepaymentDurations is > 0;

        if (isValid)
        {
            (double totalPayable, double installment) = LoanCalculator.CalculateInstallment(
                Entity.LoanAmount,
                Entity.InterestRate,
                Entity.DurationInInterestUnits,
                Entity.RepaymentDurations
            );

            Entity.InstallmentAmount = installment;
            Entity.TotalPayable = totalPayable;
        }
        else
        {
            ToastService.ShowError("Loan Amount, Rate, Durations are required to calculate installments");
        }
    }

    private async Task FindCustomerByNic()
    {
        if (!string.IsNullOrEmpty(_nic))
        {
            var response = await sender.Send(new GetLoanCustomerQuery(_nic));
            if (response.IsSuccess)
            {
                SelectedCustomer = response.Value.CustomerInfo;
                Entity.CustomerId = response.Value.CustomerId;
                Entity.LoanNumber = response.Value.LoanNumber;
            }
            else
            {
                ToastService.ShowError("Invalid NIC");
            }
        }
    }
}

internal class LoanCreateModel
{
    public string LoanNumber { get; set; }
    public Guid CustomerId { get; set; }
    public Guid LoanSchemeId { get; set; }
    public string LoanOfficer { get; set; }
    [Required]
    public double LoanAmount { get; set; }
    [Required]
    public string InterestType { get; set; }
    [Required]
    public double InterestRate { get; set; }
    public string InstallmentType { get; set; }
    [Required]
    public int DurationInInterestUnits { get; set; }
    [Required]
    public int RepaymentDurations { get; set; }
    public double InstallmentAmount { get; set; }
    public DateOnly? IssuanceDate { get; set; }
    public DateOnly? FirstInstallmentDate { get; set; }
    public string LoanDisbursementMethod { get; set; }
    public string LoanRepaymentMethod { get; set; }
    public string GuarantorsInformation { get; set; }
    public double TotalPayable { get; set; }
    public double ProcessingFee { get; set; }
    public double InsuranceAmount { get; set; }
}

