using System.Globalization;
using LoanTrack.Application.Finance.Reports.GetFinanceSummary;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Reports;

public partial class FinanceSummary(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    private DateOnly _startDate;
    private DateOnly _endDate;
    private FinanceSummaryResponse? _response;
    
    protected override void OnInitialized()
    {
        _startDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(-7));
        _endDate = DateOnly.FromDateTime(DateTime.Now);
        appSettingState.CurrentPageName = "Income Statement";
    }
    
    private async Task GetDataAsync()
    {
        var response = await sender.Send(new GetFinanceSummaryQuery(_startDate, _endDate));
        if (response.IsSuccess)
        {
            _response = response.Value;
        }
    }

    private static RenderFragment DisplayItem(string label, double amount, bool isHighlight = false) => builder =>
    {
        var culture = new CultureInfo("en-LK");
        var css =
            $"list-group-item d-flex justify-content-between align-items-center {(isHighlight ? "fw-bold bg-light" : "")}";
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", css);
        builder.AddContent(2, label);
        builder.OpenElement(3, "span");
        builder.AddAttribute(4, "class", "text-end text-nowrap");
        builder.AddContent(5, amount.ToString("C", culture));
        builder.CloseElement();
        builder.CloseElement();
    };
}

