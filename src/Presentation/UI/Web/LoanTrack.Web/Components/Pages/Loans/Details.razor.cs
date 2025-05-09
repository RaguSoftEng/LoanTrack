using System.Globalization;
using System.Text;
using Blazored.Toast.Services;
using LoanTrack.Application.Loans.Commands.IssueLoan;
using LoanTrack.Application.Loans.Commands.UpdateLoanStatus;
using LoanTrack.Application.Loans.Queries.GetLoanById;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Common;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Loans;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState,
    IToastService toastService
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private LoanViewModel Entity { get; set; } = new();
    private bool _isEditable = true;
    private LoanTrackToolbar _toobarRef;
    private string _statusCss = "status-pending";
    private StatusUpdate _statusUpdate;
    
    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Loan";
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
                Entity = new LoanViewModel(response.Value);
                if(Entity.LoanStatus is not LoanStatuses.Pending )
                    _isEditable = false;
                StateHasChanged();
            }
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        RenderStatus();
    }
    
    private static readonly CompositeFormat PaymentFormat = CompositeFormat.Parse(Paths.NextInstallmentPaymentPath);

    private void RenderStatus()
    {
        switch (Entity.LoanStatus)
        {
            case LoanStatuses.Pending:
                _statusCss = "status-pending";
                _toobarRef.AddButton("btn_approve", "Approve", "bi-pass", EventCallback.Factory.Create(this, Approve), "btn-outline-primary");
                _toobarRef.AddButton("btn_approveAndIssue", "Approve & Issue", "bi-check-square", EventCallback.Factory.Create(this, ShowIssueModal));
                _toobarRef.AddButton("btn_reject", "Reject", "bi-eraser", EventCallback.Factory.Create(this, Reject), "btn-outline-danger");
                _toobarRef.AddButton("btn_cancel", "Canceled By Customer", "bi-dash-circle", EventCallback.Factory.Create(this, Canceled), "btn-outline-danger");
                break;
            case LoanStatuses.Approved:
                _statusCss = "status-approved";
                _toobarRef.AddButton("btn_issue", "Issue", "bi-check-square", EventCallback.Factory.Create(this, ShowIssueModal), "btn-success");
                break;
            case LoanStatuses.Ongoing:
                _statusCss = "status-ongoing";
                _toobarRef.AddButton("btn_close", "Close", "bi-check-square", EventCallback.Factory.Create(this, ShowIssueModal), "btn-success");
                _toobarRef.AddButton(
                    "btn_payment",
                    "Payment",
                    "bi-paypal",
                    EventCallback.Factory.Create(this, ()=> NavManager.NavigateTo(string.Format(CultureInfo.CurrentCulture, PaymentFormat, Id)))
                );
                break;
            case LoanStatuses.Rejected or LoanStatuses.CanceledByCustomer:
                _statusCss = "status-rejected";
                break;
            case LoanStatuses.Closed or LoanStatuses.Ongoing:
                _statusCss = "status-approved";
                break;
        }

        if (Entity.LoanStatus is LoanStatuses.Ongoing or LoanStatuses.Closed)
        {
            _toobarRef.AddButton(
                "btn_installments",
                "View installments",
                "bi-list",
                EventCallback.Factory.Create(this, ()=> NavManager.NavigateTo($"Loan/{Entity.LoanId}/Installments"))
            );
        }

        StateHasChanged();
    }

    private async Task Approve()
    {
        await sender.Send(new UpdateLoanStatusCommand(Entity.LoanId, LoanStatuses.Approved));
        ReloadPage();
    }

    private void ShowIssueModal()
    {
        _statusUpdate.Show();
    }
    
    private async Task Reject()
    {
        await sender.Send(new UpdateLoanStatusCommand(Entity.LoanId, LoanStatuses.Rejected));
        ReloadPage();
    }
    private async Task Canceled()
    {
        await sender.Send(new UpdateLoanStatusCommand(Entity.LoanId, LoanStatuses.CanceledByCustomer));
        ReloadPage();
    }
    
    private async Task SubmitStatus(LoanViewModel entity)
    {
        if (Entity.LoanStatus is LoanStatuses.Pending or LoanStatuses.Approved)
        {
            if (Entity.FirstInstallmentDate == null
                || Entity.IssuanceDate == null
                || !(entity.IssuanceDate > DateOnly.MinValue && entity.IssuanceDate < DateOnly.MaxValue)
                || !(entity.FirstInstallmentDate > DateOnly.MinValue && entity.FirstInstallmentDate < DateOnly.MaxValue)
               )
            {
                toastService.ShowError("Please select valid date");
                return;
            }
            var response =  await sender.Send(new IssueLoanCommand(Entity.LoanId, entity.IssuanceDate!.Value, Entity.FirstInstallmentDate!.Value));
            if (response.IsFailure)
            {
                toastService.ShowError(response.Error.Description);
            }
        }
        else if(Entity.LoanStatus is LoanStatuses.Ongoing)
        {
          var response =  await sender.Send(new UpdateLoanStatusCommand(Entity.LoanId, LoanStatuses.Closed, Entity.ClosedDate!.Value));
          if (response.IsFailure)
          {
              toastService.ShowError(response.Error.Description);
          }
        }
        _statusUpdate.Close();
        ReloadPage();
    }
    
    private void ReloadPage()
    {
        NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
    }
    private static string GetBaseUnit(string interestType) => interestType switch
    {
        InterestTypes.PerDay => "Days",
        InterestTypes.PerMonth => "Months",
        InterestTypes.PerWeek => "Weeks",
        InterestTypes.PerAnnum => "Years",
        _ => "Months"
    };
    
}

public class LoanViewModel
{
    public Guid LoanId { get; set; }
    public string LoanNumber { get; set; }
    public string Customer { get; set; }
    public string LoanScheme { get; set; }
    public string LoanOfficer { get; set; }
    public double LoanAmount { get; set; }
    public string InterestType { get; set; }
    public double InterestRate { get; set; }
    public string InstallmentType { get; set; }
    public int DurationInInterestUnits {get;set;}
    public int RepaymentDurations { get; set; }
    public double InstallmentAmount { get; set; }
    public DateOnly? IssuanceDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public DateOnly? FirstInstallmentDate { get; set; }
    public string LoanDisbursementMethod { get; set; }
    public string LoanRepaymentMethod { get; set; }
    public string GuarantorsInformation { get; set; }
    public string LoanStatus { get; set; }
    public DateOnly? ClosedDate { get; set; }
    public double TotalAmountPayable { get; set; }
    public double Arrears  { get; set; }
    public double ProcessingFee { get; set; }
    public double InsuranceAmount { get; set; }

    public LoanViewModel()
    {
        
    }

    public LoanViewModel(GetLoanByIdResponse response)
    {
        LoanId = response.LoanId;
        LoanNumber = response.LoanNumber;
        Customer = response.Customer;
        LoanScheme = response.LoanScheme;
        LoanOfficer = response.LoanOfficer;
        LoanAmount = response.LoanAmount;
        InterestType = response.InterestType;
        InterestRate = response.InterestRate;
        InstallmentType = response.InstallmentType;
        RepaymentDurations = response.RepaymentDurations;
        InstallmentAmount = response.InstallmentAmount;
        IssuanceDate = response.IssuanceDate;
        EndDate = response.EndDate;
        FirstInstallmentDate = response.NextInstallmentDate;
        LoanDisbursementMethod = response.LoanDisbursementMethod;
        LoanRepaymentMethod = response.LoanRepaymentMethod;
        GuarantorsInformation = response.GuarantorsInformation;
        LoanStatus = response.LoanStatus;
        ClosedDate = response.ClosedDate;
        TotalAmountPayable = response.TotalAmountPayable;
        Arrears = response.TotalAmountPayable - response.PaidAmount;
        DurationInInterestUnits = response.DurationInInterestUnits;
        ProcessingFee = response.ProcessingFee;
        InsuranceAmount = response.InsuranceAmount;
    }
}
