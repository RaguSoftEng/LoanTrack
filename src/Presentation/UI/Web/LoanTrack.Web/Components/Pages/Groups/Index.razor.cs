using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups.Queries.GetGroups;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LoanTrack.Web.Components.Pages.Groups;

public partial class Index(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{
    private IReadOnlyCollection<GetGroupsResponse> _groups = [];
    private List<ListValueResponse> _centers = [];
    private Guid SelectedCenter {get;set;} = Guid.Empty;
    
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
        appSetting.CurrentPageName = "Groups";
        await LoadCentersAsync();
        await LoadGroupsAsync(Guid.Empty);
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
    
    private async Task OpenAdvanceFilter()
    {
        await Js.InvokeVoidAsync("modalHelper.show", "filterModal");
    }
    
    private async Task CloseAdvanceFilter()
    {
        await Js.InvokeVoidAsync("modalHelper.hide", "filterModal");
    }
    
    private async Task ChangePage(int page)
    {
        _currentPage = page;
        await LoadGroupsAsync(SelectedCenter);
    }

    private async Task LoadCentersAsync()
    {
        var centersResponse = await sender.Send(new GetCentersListValueQuery());
        if (centersResponse.IsSuccess)
        {
            _centers = [..centersResponse.Value];
        }
    }

    private async Task OnChangeSelectedCenter(Guid id)
    {
        SelectedCenter = id;
        await LoadGroupsAsync(id);
    }

    private async Task ApplyFilter()
    {
        _currentPage = 1;
        await LoadGroupsAsync(SelectedCenter);
        await CloseAdvanceFilter();
    }

    private async Task LoadGroupsAsync(Guid centerId)
    {
        var query = new GetGroupsQuery
        {
            Center = centerId,
            Page = _currentPage,
            PageSize = _pageSize,
            SearchBy = _searchBy,
            Search = _searchTerm,
            SortBy = _shortBy
        };
        var response = await sender.Send(query);
        if (response.IsSuccess)
        {
            var results = response.Value;
            _groups = [.. results.Items];
            _totalPages = results.TotalPages;
            _pageSize = results.PageSize;
            _currentPage = results.Page;
        }
    }
}

