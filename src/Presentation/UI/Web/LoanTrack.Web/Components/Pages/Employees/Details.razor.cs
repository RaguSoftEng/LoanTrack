using LoanTrack.Application.Employees.Queries.GetUser.ById;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Employees;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Employees;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private EmployeeVm Entity { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Customer";
        await GetEmployeeDetails();
    }

    private async Task GetEmployeeDetails()
    {
        var employeeId = Guid.Parse(Id);
        var response = await sender.Send(new GetUserQuery(employeeId));
        if (response.IsSuccess)
        {
            var result = response.Value;
            Entity = new EmployeeVm()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                UserRole = result.UserRole,
                IsActive = result.IsActive
            };
        }
    }
}

