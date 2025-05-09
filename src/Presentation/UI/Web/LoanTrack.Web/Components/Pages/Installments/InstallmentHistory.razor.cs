using LoanTrack.Application.Loans.Queries.Installments;
using LoanTrack.Application.Loans.Queries.Installments.GetInstallments;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Installments;

public partial class InstallmentHistory(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    [Parameter] public string LoanId { get; set; } = string.Empty;
    private List<InstallmentsListResponse>? _installments;

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Loan Installments";
        await GetInstallmentsAsync();
    }

    private async Task GetInstallmentsAsync()
    {
        var loanId = Guid.Parse(LoanId);
        var response = await sender.Send(new GetInstallmentsQuery(loanId));
        if (response.IsSuccess)
        {
            _installments = [..response.Value];
        }
    }
}

