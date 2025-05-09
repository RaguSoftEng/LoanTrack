using LoanTrack.Application.LoanSchemes.Queries.GetList;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Schemes;

public partial class Index(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    private IReadOnlyCollection<LoanSchemeListResponse> _schemes = [];

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Loan Schemes";
        await GetAllLoanSchemes();
    }

    private async Task GetAllLoanSchemes()
    {
        var response = await sender.Send(new GetAllLoanSchemesQuery());
        if (response.IsSuccess)
        {
            _schemes = response.Value;
        }
    }
}

