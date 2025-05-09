using System.Globalization;
using System.Text;
using Blazored.Toast.Services;
using LoanTrack.Application.Loans.Commands.ReceiveInstalment;
using LoanTrack.Application.Loans.Queries.Installments.GetInstallment;
using LoanTrack.Application.Loans.Queries.Installments.GetNextInstallment;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Installments;

public partial class Payment(
    ISender sender,
    AppSettingState appSettingState,
    ToastService toastService
) : ComponentBase
{
    [Parameter] public string LoanId { get; set; }
    [Parameter] public string Id { get; set; }

    private InstallmentViewModel _entity = new ();

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Installment Payment";
        await GetInstallment();
    }

    private async Task OnSubmit()
    {
        if (_entity.PaidAmount >= _entity.InstallmentAmount)
        {
            _entity.PaymentDate = _entity.PaymentDate >= DateOnly.MinValue && _entity.PaymentDate <= DateOnly.MaxValue
                ? _entity.PaymentDate
                : DateOnly.FromDateTime(DateTime.UtcNow);
            var response = await sender.Send(new ReceiveInstallmentCommand(
                _entity.InstallmentId,
                _entity.PaidAmount,
                _entity.PaymentDate,
                _entity.IsDelayed,
                _entity.DelayedDays,
                _entity.PaymentMethod,
                _entity.IsPenaltyApplied,
                _entity.PenaltyAmount,
                _entity.PenaltyReason,
                _entity.PaymentDescription
            ));

            if (response.IsSuccess)
            {
                NavManager.NavigateTo(string.Format(
                    CultureInfo.CurrentCulture,
                    CompositeFormat.Parse(Paths.InstallmentView),
                    _entity.LoanId,
                    _entity.InstallmentId
                ));
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }

    private async Task GetInstallment()
    {
        if (string.IsNullOrEmpty(Id))
        {
            var response = await sender.Send(new GetNextInstallmentQuery(Guid.Parse(LoanId)));
            if (response.IsSuccess)
            {
                Id = response.Value.InstallmentId.ToString();
                var installment = response.Value;
                _entity = new InstallmentViewModel
                {
                    LoanId = installment.LoanId,
                    InstallmentId = installment.InstallmentId,
                    LoanNumber = installment.LoanNumber,
                    InstallmentNumber = installment.InstallmentNumber,
                    InstallmentDate = installment.InstallmentDate,
                    InstallmentAmount = installment.InstallmentAmount,
                    IsDelayed = installment.IsDelayed,
                    DelayedDays = installment.DelayedDays,
                    PaidAmount = installment.InstallmentAmount
                };
                StateHasChanged();
            }
        }
        else
        {
            var response = await sender.Send(new GetInstallmentByIdQuery(Guid.Parse(Id)));
            if (response.IsSuccess)
            {
                var installment = response.Value;
                _entity = new InstallmentViewModel
                {
                    LoanId = installment.LoanId,
                    InstallmentId = installment.InstallmentId,
                    LoanNumber = installment.LoanNumber,
                    InstallmentNumber = installment.InstallmentNumber,
                    InstallmentDate = installment.InstallmentDate,
                    InstallmentAmount = installment.InstallmentAmount,
                    IsDelayed = installment.IsDelayed,
                    DelayedDays = installment.DelayedDays,
                    PaidAmount = installment.InstallmentAmount
                };
                StateHasChanged();
            }
        }
    }

    private void IncludePenalty(double value)
    {
        _entity.PenaltyAmount = value;
        _entity.PaidAmount = _entity.InstallmentAmount + value;
    }
}

internal class InstallmentViewModel
{
    public Guid InstallmentId { get; set; }
    public Guid LoanId { get; set; }
    public string LoanNumber { get; set; } = string.Empty;
    public int InstallmentNumber { get; set; }
    public DateOnly InstallmentDate { get; set; }
    public double InstallmentAmount { get; set; }
    public bool IsDelayed { get; set; }
    public int DelayedDays { get; set; }
    public bool IsPenaltyApplied { get; set; }
    public double PenaltyAmount { get; set; }   
    public string PenaltyReason { get; set; }
    public DateOnly PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentDescription { get; set; }
    public double PaidAmount { get; set; }
}
