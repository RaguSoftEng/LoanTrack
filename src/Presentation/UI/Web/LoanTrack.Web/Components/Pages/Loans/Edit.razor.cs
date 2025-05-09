using System.ComponentModel.DataAnnotations;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Loans.Commands.UpdateLoan;
using LoanTrack.Application.Loans.Queries.GetLoanById;
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

public partial class Edit(
    ISender sender,
    AppSettingState appSetting,
    NavigationManager navigationManager
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private LoanEditModel Entity { get; set; } = new();
    private Guid _selectedScheme  = Guid.Empty;
    private List<string> _interestTypes = [];
    private List<string> _installmentTypes = [];
    private List<string> _disbursementMethods = [];
    private List<string> _repaymentMethods = [];
    private LoanSchemeResponse? SelectedScheme { get; set; }
    private readonly List<ListValueResponse> _schemes = [];

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "New Loan";
        await GetLoanAsync();
    }
    
    private async Task GetLoanAsync()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var loanId = Guid.Parse(Id);
            var response = await sender.Send(new GetLoanByIdQuery(loanId));
            if (response.IsSuccess)
            {
                Entity = new LoanEditModel(response.Value);
                await LoadListValues();
                StateHasChanged();
            }
        }
    }

    private async Task LoadListValues()
    {
        _interestTypes = [.. InterestTypes.GetTypes];
        GetInstallmentTypes(null);
        _disbursementMethods = [..LoanDisbursementMethods.GetMethods];
        _repaymentMethods = [..LoanRepaymentMethods.GetMethods];
        await LoadSchemesAsync();
    }

    private async Task OnSubmit()
    {
        var result = await sender.Send(new UpdateLoanCommand(
            Entity.LoanId,
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
            navigationManager.NavigateTo($"{Paths.LoanView}/{Entity.LoanId}");
        }
        else
        {
            ToastService.ShowError(result.Error.Description);
        }
    }
    
    private void OnInstallmentTypeChanged(string value)
    {
        Entity.InstallmentType = value;
        Entity.RepaymentDurations = LoanCalculator.CalDuration(Entity.InterestType, value, Entity.DurationInInterestUnits);
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

    private async Task LoadSchemesAsync()
    {
       var response = await sender.Send(new GetSchemesAsListValuesQuery());
       _schemes.Add(new ListValueResponse(
           Guid.Empty,
           "Custom",
           Entity.LoanSchemeId == Guid.Empty || Entity.LoanSchemeId == null
        ));
       
       if (response.IsSuccess)
       {
           var schemes = response.Value
               .Select(x=> x with{ IsSelected = Equals(x.Id, Entity.LoanSchemeId)})
               .ToList();
           _schemes.AddRange([..schemes]);
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

    private void GetInstallmentTypes(string? value)
    {
        Entity.InterestType = value ?? Entity.InterestType;
        _installmentTypes = Entity.InterestType switch
        {
            InterestTypes.PerMonth => [InstallmentTypes.Monthly, InstallmentTypes.Weekly, InstallmentTypes.Daily],
            InterestTypes.PerWeek => [InstallmentTypes.Weekly, InstallmentTypes.Daily],
            InterestTypes.PerAnnum =>
                [InstallmentTypes.Yearly, InstallmentTypes.Monthly, InstallmentTypes.Weekly, InstallmentTypes.Daily],
            InterestTypes.PerDay => [InstallmentTypes.Daily],
            _ => [..InstallmentTypes.GetTypes]
        };
        OnInstallmentTypeChanged(value is null ? Entity.InstallmentType : _installmentTypes[0]);
        StateHasChanged();
    }

    private void CalClicked()
    {
        bool isValid = Entity.InterestRate is > 0 and < 100
                       && Entity.DurationInInterestUnits is > 0
                       && Entity.RepaymentDurations is > 0;
        if(!isValid) ToastService.ShowError("Loan Amount, Rate, Durations are required to calculate installments");
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
            Entity.InstallmentAmount = 0;
        }
    }
}

internal class LoanEditModel
{
    public Guid LoanId { get; set; }
    public string LoanNumber { get; set; }
    public string Customer { get; set; }
    public Guid? LoanSchemeId { get; set; }
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
    public string LoanStatus { get; set; }
    public double ProcessingFee { get; set; }
    public double InsuranceAmount { get; set; }
    
    public LoanEditModel()
    {
        
    }

    public LoanEditModel(GetLoanByIdResponse response)
    {
        LoanId = response.LoanId;
        LoanNumber = response.LoanNumber;
        Customer = response.Customer;
        LoanSchemeId = response.SchemeId;
        LoanOfficer = response.LoanOfficer;
        LoanAmount = response.LoanAmount;
        InterestType = response.InterestType;
        InterestRate = response.InterestRate;
        InstallmentType = response.InstallmentType;
        RepaymentDurations = response.RepaymentDurations;
        InstallmentAmount = response.InstallmentAmount;
        IssuanceDate = response.IssuanceDate;
        FirstInstallmentDate = response.NextInstallmentDate;
        LoanDisbursementMethod = response.LoanDisbursementMethod;
        LoanRepaymentMethod = response.LoanRepaymentMethod;
        GuarantorsInformation = response.GuarantorsInformation;
        LoanStatus = response.LoanStatus;
        DurationInInterestUnits = response.DurationInInterestUnits;
        TotalPayable = response.TotalAmountPayable;
        ProcessingFee = response.ProcessingFee;
        InsuranceAmount = response.InsuranceAmount;
    }
}

