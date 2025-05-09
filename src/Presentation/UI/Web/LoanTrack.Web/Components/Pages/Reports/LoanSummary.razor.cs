using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Finance.Reports.LoanSummary;
using LoanTrack.Application.Groups.Queries.GetGroupsListValues;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Reports;

public partial class LoanSummary(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{
    private bool _includeClosed;
    private List<ListValueResponse> _centers = [];
    private Guid _selectedCenterId = Guid.Empty;
    private List<ListValueResponse> _groups = [];
    private Guid _selectedGroupId = Guid.Empty;
    private string _customerNic = string.Empty;
    private IReadOnlyList<LoanSummaryResponse>? _loanSummary;
    
    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Loans";
        _customerNic = "";
        _includeClosed = false;
        await LoadCentersAsync();
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
        _groups =[];
        if (centerId != Guid.Empty)
        {
            var centersResponse = await sender.Send(new GetGroupsListValueQuery(centerId));
           
            if (centersResponse.IsSuccess)
            {
                _groups.AddRange([..centersResponse.Value]);
            }
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

    private async Task ApplyFilterAsync()
    {
        var response = await sender.Send(new GetLoanSummaryQuery(
            _selectedCenterId,
            _selectedGroupId,
            _customerNic,
            _includeClosed
        ));
        if (response.IsSuccess)
        {
            _loanSummary = response.Value;
        }
    }
}

