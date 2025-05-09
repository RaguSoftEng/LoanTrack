using System.Globalization;
using LoanTrack.Application.Dashboard;
using LoanTrack.Application.Finance.Reports.GetFinanceSummary;
using LoanTrack.Application.Loans.Queries.Reports.GetCollection;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Dashboard;

public partial class Index(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{

    private Dictionary<string, List<(string Center, string Group, int Count)>>? _customersCountsByCentre;
    private IReadOnlyCollection<(string Status, int Count)>? _loanCountsByStatus;
    private (int DueToday, int Overdue) _installmentCounts = (0, 0);
    private CollectionResponse? _collections;
    private FinanceSummaryResponse? _financeSummary;
    private bool _isAdmin;

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Dashboard";
        _isAdmin = await CurrentUser.IsInAnyRole(EmployeeRoles.Admin);
        await GetDashboardDataAsync();
    }

    private async Task GetDashboardDataAsync()
    {
        var response = await sender.Send(new GetDashboardQuery());
        if (response.IsSuccess)
        {
            var dashboardData = response.Value;
            _customersCountsByCentre = dashboardData.CustomersCountsByCentre
                .GroupBy(x => x.Center)
                .ToDictionary(x => x.Key, x => x.ToList());
            _loanCountsByStatus = dashboardData.LoanCountsByStatus;
            _installmentCounts = dashboardData.InstallmentCounts;
            _collections = dashboardData.Collections;
            _financeSummary = dashboardData.FinanceSummary;
        }
    }

    private static RenderFragment DisplaySummaryItem(string label, double value, string icon, string color) =>
        builder =>
        {
            var culture = new CultureInfo("en-LK");
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "col-12 col-md-6 col-lg-4 col-xl-3");

            builder.OpenElement(2, "div");
            builder.AddAttribute(3, "class", $"card border-left-{color} shadow-sm h-100");

            builder.OpenElement(4, "div");
            builder.AddAttribute(5, "class", "card-body");

            builder.OpenElement(6, "div");
            builder.AddAttribute(7, "class", "d-flex align-items-center gap-2");

            builder.OpenElement(8, "i");
            builder.AddAttribute(9, "class", $"{icon} text-{color} fs-4");
            builder.CloseElement(); // i

            builder.OpenElement(10, "div");

            builder.OpenElement(11, "h6");
            builder.AddContent(12, label);
            builder.CloseElement(); // h6

            builder.OpenElement(13, "p");
            builder.AddAttribute(14, "class", "mb-0 fw-bold fs-5");
            builder.AddContent(15, value.ToString("C", culture)); // Format as currency
            builder.CloseElement(); // p

            builder.CloseElement(); // div
            builder.CloseElement(); // d-flex

            builder.CloseElement(); // card-body
            builder.CloseElement(); // card
            builder.CloseElement(); // col
        };
}

