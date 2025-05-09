using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Application.Customers.Queries.GetCustomers;
using LoanTrack.Application.Groups.Queries.GetGroupsListValues;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LoanTrack.Web.Components.Pages.Customers;

public partial class Index(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{
    private IReadOnlyCollection<CustomerResponse> Customers { get; set; } = [];
    private List<ListValueResponse> _centers = [];
    private Guid _selectedCenterId = Guid.Empty;
    private List<ListValueResponse> _groups = [];
    private Guid _selectedGroupId = Guid.Empty;
    
    private LoanTrackToolbar _toolbar;
    private string _searchBy = "FullName";
    private string _searchTerm = string.Empty;
    private string _shortBy = "FullName";
    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalPages = 1;
    
    private List<int> VisiblePages => [.. Enumerable.Range(1, _totalPages)
        .Skip(Math.Max(0, _currentPage - 3))
        .Take(5)];

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Customers";
        await LoadCentersAsync();
        await LoadCustomersAsync();
    }
    
    private async Task LoadCentersAsync()
    {
        var centersResponse = await sender.Send(new GetCentersListValueQuery());
        _centers = [];
        if (centersResponse.IsSuccess)
        {
            _centers.AddRange([..centersResponse.Value]);
        }
    }

    private async Task OnCenterSelected(Guid centerId)
    {
        _selectedCenterId = centerId;
        await LoadGroupsByCenterAsync(_selectedCenterId);
    }
    
    private void OnGroupSelected(Guid groupId) => _selectedGroupId = groupId;

    private async Task LoadGroupsByCenterAsync(Guid centerId)
    {
        _groups = [];
        if (centerId != Guid.Empty)
        {
            var centersResponse = await sender.Send(new GetGroupsListValueQuery(centerId));
           
            if (centersResponse.IsSuccess)
            {
                _groups.AddRange([..centersResponse.Value]);
            }
        }
    }

    private async Task LoadCustomersAsync()
    {
        var query = new GetCustomersQuery{
            CenterId = _selectedCenterId,
            GroupId = _selectedGroupId,
            Page = _currentPage,
            PageSize = _pageSize,
            Search = _searchTerm,
            SearchBy = _searchBy,
            SortBy = _shortBy
        };
        
        var response = await sender.Send(query);
        if (response.IsSuccess)
        {
            var results = response.Value;
            Customers = results.Items;
            _totalPages = results.TotalPages;
            _pageSize = results.PageSize;
            _currentPage = results.Page;
        }
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
        await LoadCustomersAsync();
    }

    private async Task ApplyFilter()
    {
        _currentPage = 1;
        await LoadCustomersAsync();
        await CloseAdvanceFilter();
    }
}

