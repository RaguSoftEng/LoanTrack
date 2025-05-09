using LoanTrack.Application.Centers.Queries.Get;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LoanTrack.Web.Components.Pages.Centers;

public partial class Index(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{
    private IEnumerable<GetCentersResponse> _centers = [];
    private LoanTrackToolbar _toolbar;
    private string _searchBy = "name";
    private string _searchTerm = string.Empty;
    private string _shortBy = "name";
    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalPages = 1;
    
    private List<int> VisiblePages => [.. Enumerable.Range(1, _totalPages)
        .Skip(Math.Max(0, _currentPage - 3))
        .Take(5)];

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Centers";
        await LoadCentersAsync();
    }

    protected override void OnAfterRender(bool firstRender){
        if (firstRender)
        {
            _toolbar.AddButton(
                "btn_filter",
                "Filter",
                "bi-filter",
                EventCallback.Factory.Create(this, OpenAdvanceFilter),
                attributes:new Dictionary<string, object>
                {
                    { "data-bs-target", "#filterModal" },
                    { "data-bs-toggle", "#modal" }
                }
            );
        }
    }

    private async Task ChangePage(int page)
    {
        _currentPage = page;
        await LoadCentersAsync();
    }

    private async Task OpenAdvanceFilter()
    {
        await Js.InvokeVoidAsync("modalHelper.show", "filterModal");
    }
    
    private async Task CloseAdvanceFilter()
    {
        await Js.InvokeVoidAsync("modalHelper.hide", "filterModal");
    }
    
    private async Task ApplyFilter()
    {
        _currentPage = 1;
        await LoadCentersAsync();
        await CloseAdvanceFilter();
    }
    
    private async Task LoadCentersAsync()
    {
        var query = new GetCentersQuery
        {
            Page = _currentPage,
            PageSize = _pageSize,
            SearchBy = _searchBy,
            Search = _searchTerm,
            SortBy = _shortBy,
            SortDescending = false
        };
        var response = await sender.Send(query);
        if (response.IsSuccess)
        {
            var results = response.Value;
            _centers = [..results.Items];
            _totalPages = results.TotalPages;
            _pageSize = results.PageSize;
            _currentPage = results.Page;
        }
    }
}

