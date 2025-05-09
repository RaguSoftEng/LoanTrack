using Blazored.Toast.Services;
using LoanTrack.Application.Employees.Commands.Update;
using LoanTrack.Application.Employees.Queries.GetUser.ById;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Employees;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Employees;

public partial class Edit(
    ISender sender,
    AppSettingState appSetting,
    IToastService toastService,
    NavigationManager navigationManager
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private EmployeeVm Entity { get; set; } = new();
    private static IReadOnlyCollection<string> UserRoles => EmployeeRoles.GetRoles;

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Customer";
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
                UserId = result.UserId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                UserRole = result.UserRole,
                IsActive = result.IsActive
            };
        }
    }

    private async Task OnSubmit()
    {
        var response = await sender.Send(new UpdateEmployeeCommand(
            Entity.UserId,
            Entity.FirstName,
            Entity.LastName,
            Entity.UserRole
        ));

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Profile updated Successfully...");
            navigationManager.NavigateTo($"{Paths.EmployeeView}/{Entity.UserId}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

