using System.Globalization;
using System.Text;
using System.Xml;
using LoanTrack.Application.Loans.Queries.Installments;
using LoanTrack.Application.Loans.Queries.Installments.GetInstallment;
using LoanTrack.Web.Common;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Installments;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    [Parameter] public string LoanId { get; set; }
    [Parameter] public string Id { get; set; }

    private GetInstallmentResponse? _entity;
    public LoanTrackToolbar ToolbarRef;

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Installment";
        await GetInstallment();
    }

    private async Task GetInstallment()
    {
        var response = await sender.Send(new GetInstallmentByIdQuery(Guid.Parse(Id)));
        if (response.IsSuccess)
        {
            _entity = response.Value;
            if (!_entity.IsPaid)
            {
                ToolbarRef.AddButton(
                    "btn_payment",
                    "Payment",
                    "bi-paypal",
                    EventCallback.Factory.Create(this, ()=> NavManager.NavigateTo(string.Format(CultureInfo.CurrentCulture, PaymentFormat, LoanId, Id)))
                );
            }
            StateHasChanged();
        }
    }
    private static readonly CompositeFormat PaymentFormat = CompositeFormat.Parse(Paths.InstallmentPaymentPath);

    private void ShowPayment()
    {
        NavManager.NavigateTo(string.Format(CultureInfo.CurrentCulture, PaymentFormat, LoanId, Id));
    }
}

