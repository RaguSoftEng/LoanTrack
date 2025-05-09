using Blazored.Toast.Services;
using LoanTrack.Application.Employees.Commands.RegisterUser;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Employees;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Employees;

public partial class Create(
    ISender sender,
    AppSettingState appSetting,
    IToastService toastService,
    NavigationManager navigationManager
) : ComponentBase
{
    private EmployeeVm Entity { get; set; } = new();
    private static IReadOnlyCollection<string> UserRoles => EmployeeRoles.GetRoles;

    protected override void OnInitialized()
    {
        appSetting.CurrentPageName = "New Employee";
    }

    private void OnClear () => Entity = new EmployeeVm();
    
    private async Task OnSubmit()
    {
        var response = await sender.Send(
            new RegisterUserCommand(
                Entity.FirstName,
                Entity.LastName,
                Entity.Email,
                Entity.Password,
                Entity.UserRole
            )
        );

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Employee Created Successfully...");
            navigationManager.NavigateTo($"{Paths.EmployeeView}/{response.Value}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

