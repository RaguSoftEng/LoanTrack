using LoanTrack.Application.Loans.Queries.Reports.GetCollection;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Reports;

public partial class Collections(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    private DateOnly _startDate;
    private DateOnly _endDate;
    private CollectionResponse? _collection;

    protected override void OnInitialized()
    {
        _startDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(-7));
        _endDate = DateOnly.FromDateTime(DateTime.Now);
        appSettingState.CurrentPageName = "Collection Report";
    }

    private async Task GetDataAsync()
    {
        var response = await sender.Send(new GetCollectionQuery(_startDate, _endDate));
        if (response.IsSuccess)
        {
            _collection = response.Value;
        }
    }
}

