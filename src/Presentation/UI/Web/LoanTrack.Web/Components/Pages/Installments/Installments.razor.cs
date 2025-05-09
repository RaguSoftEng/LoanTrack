using System.Globalization;
using System.Text;
using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups.Queries.GetGroupsListValues;
using LoanTrack.Application.Loans.Commands.ReceiveBulkInstallment;
using LoanTrack.Application.Loans.Queries.Installments.GetInstallmentsByCustomer;
using LoanTrack.Application.Loans.Queries.Installments.GetNextInstallments;
using LoanTrack.Web.Common;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Loans;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Installments;

public partial class Installments(
    ISender sender,
    AppSettingState appSettingState,
    IToastService toastService
) : ComponentBase
{
    private string _nic = string.Empty;
    private List<InstallmentVm>? _installments;
    private List<ListValueResponse> _centers = [];
    private Guid _selectedCenterId = Guid.Empty;
    private List<ListValueResponse> _groups = [];
    private Guid _selectedGroupId = Guid.Empty;
    private bool _isFiltered;
    private LoanTrackToolbar _toolbar;
    private DateOnly? _paymentDate;

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Installments";
        await GetAllCenters();
    }

    private async Task GetNextInstallmentsAsync()
    {
        var response = await sender.Send(new GetNextInstallmentsQuery(_selectedCenterId, _selectedGroupId));
        if (response.IsSuccess)
        {
            _installments = [.. response.Value.Select(InstallmentVm.LoadByInstallmentResponse)];
            _isFiltered = true;
            StateHasChanged();
            AddPaymentButton();
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }

    private string PaymentTotal =>
        _installments?.Sum(x => x.PaymentAmount).ToString("##.00", CultureInfo.InvariantCulture);
    
    private async Task FindCustomerByNic()
    {
        if (!string.IsNullOrEmpty(_nic))
        {
            var response = await sender.Send(new GetInstallmentsByCustomerNicQuery(_nic));
            if (response.IsSuccess)
            {
                _installments = [.. response.Value.Select(InstallmentVm.LoadByInstallmentResponse)];
                _isFiltered = true;
                if (_installments is { Count: 1 })
                {
                    var entity = _installments[0];
                    NavManager.NavigateTo(string.Format(CultureInfo.CurrentCulture, CompositeFormat.Parse(Paths.InstallmentPaymentPath), entity.LoanId, entity.InstallmentId));
                }
                else
                {
                    StateHasChanged();
                    AddPaymentButton();
                }
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }

    private void ClearCustomer()
    {
        _nic = string.Empty;
    }

    private async Task GetAllCenters()
    {
        _centers = [];
        var response = await sender.Send(new GetCentersListValueQuery());
        if (response.IsSuccess)
        {
            _centers.AddRange([..response.Value]);
        }
    }

    private async Task OnCenterSelected(Guid centerId)
    {
        _selectedCenterId = centerId;
        await GetGroupsByCenter(centerId);
        StateHasChanged();
    }

    private async Task GetGroupsByCenter(Guid centerId)
    {
        _groups = [];
        if (centerId != Guid.Empty)
        {
            var center = _centers.Find(x => Equals(x.Id, centerId));
            var response = await sender.Send(new GetGroupsListValueQuery(centerId));
            if (response.IsSuccess)
            {
                _groups.AddRange([..response.Value]);
            }
        }
    }

    private void OnBackClicked()
    {
        RemovePaymentButton();
        if (!_isFiltered)
        {
            return;
        }

        _isFiltered = false;
        _installments = null;
    }

    private void OnSubmitClicked()
    {
        var data = _installments;
    }

    private void AddPaymentButton()
    {
        _toolbar.AddButton("btn_payAll", "Pay All", "bi-paypal", EventCallback.Factory.Create(this, BulkPayments));
    }

    private void RemovePaymentButton()
    {
        _toolbar.RemoveButton("btn_payAll");
    }

    private async Task BulkPayments()
    {
        var payments = _installments
            ?.Select(x => (x.InstallmentId, x.PaymentAmount, x.PaymentDate, x.IsDelayed, x.DaysDelayed)).ToList();
        if (payments is { Count: > 0 })
        {
            var response = await sender.Send(new ReceiveBulkInstallmentCommand(payments));
            if (response.IsSuccess)
            {
                _installments?.ForEach(x=>x.IsPaid = true);
                StateHasChanged();
                RemovePaymentButton();
                toastService.ShowSuccess($"Successfully paid {payments.Count} installments");
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }

    private void UpdatePaymentDate(DateOnly? date)
    {
        if (!date.HasValue)
        {
            return;
        }

        _paymentDate = date;
        _installments?.ForEach(x => x.PaymentDate = _paymentDate.Value);
    }

    private void RemoveInstallment(Guid id)
    {
        _installments?.RemoveAll(x=>x.InstallmentId==id);
        StateHasChanged();
    }
}

