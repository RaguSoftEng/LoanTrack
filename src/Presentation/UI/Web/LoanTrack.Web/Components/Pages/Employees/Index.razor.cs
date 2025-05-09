using LoanTrack.Application.Employees.Queries.GetUsers;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Employees;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Employees;

public partial class Index(
    ISender sender,
    AppSettingState appSetting
) : ComponentBase
{
    private IReadOnlyList<EmployeeVm> _employees { get; set; } = [];
    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalPages = 1;
    
    private List<int> VisiblePages => [.. Enumerable.Range(1, _totalPages)
        .Skip(Math.Max(0, _currentPage - 3))
        .Take(5)];
    
    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Employees";
        await GetEmployeesAsync();
    }
    private async Task ChangePage(int page)
    {
        _currentPage = page;
        await GetEmployeesAsync();
    }

    private async Task GetEmployeesAsync()
    {
        var query = new GetUsersQuery{
            Page = _currentPage,
            PageSize = _pageSize,
            SortBy = "FirstName"
        };
        var response = await sender.Send(query);
        if (response.IsSuccess)
        {
            var results = response.Value;
            _employees = EmployeeVm.LoadEmployees(results.Items);
            _totalPages = results.TotalPages;
            _pageSize = results.PageSize;
            _currentPage = results.Page;
        }
    }
}

