using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups.Queries.GetGroupsListValues;
using LoanTrack.Application.Loans.Queries.GetLoans;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Components.Common;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LoanTrack.Web.Components.Pages.Loans;

public partial class Index(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{
    private List<GetLoansResponse> Loans { get; set; } = [];
    private List<ListValueResponse> _centers = [];
    private Guid _selectedCenterId = Guid.Empty;
    private List<ListValueResponse> _groups = [];
    private Guid _selectedGroupId = Guid.Empty;
    private string _customerNic = string.Empty;

    private LoanTrackToolbar _toolbar;
    private string _searchBy = "LoanNumber";
    private string _searchTerm = string.Empty;
    private string _shortBy = "LoanNumber";
    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalPages = 1;

    private List<int> VisiblePages =>
    [
        .. Enumerable.Range(1, _totalPages)
            .Skip(Math.Max(0, _currentPage - 3))
            .Take(5)
    ];

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Loan Summary Report";
        await LoadCentersAsync();
        await GetLoansAsync();
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

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            _toolbar.AddButton(
                "btn_filter",
                "Filter",
                "bi-filter",
                EventCallback.Factory.Create(this, OpenAdvanceFilter),
                attributes: new Dictionary<string, object>
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
        await GetLoansAsync();
    }

    private async Task ApplyFilter()
    {
        _currentPage = 1;
        await GetLoansAsync();
        await CloseAdvanceFilter();
    }

    private async Task GetLoansAsync()
    {
        var query = new GetLoansQuery
        {
            CenterId = _selectedCenterId,
            GroupId = _selectedGroupId,
            Nic = _customerNic,
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
            Loans = [..results.Items];
            _totalPages = results.TotalPages;
            _pageSize = results.PageSize;
            _currentPage = results.Page;
        }
    }

    private static string GetStatus(string status) => status switch
    {
        LoanStatuses.Pending => "status-pending",
        LoanStatuses.Approved => "status-approved",
        LoanStatuses.Ongoing => "status-ongoing",
        LoanStatuses.Rejected or LoanStatuses.CanceledByCustomer => "status-rejected",
        LoanStatuses.Closed => "status-approved",
        _ => "status-approved"
    };
}

