using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups.Commands.Create;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Groups;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Groups;

public partial class Create(
    ISender sender,
    IToastService toastService,
    AppSettingState appSetting,
    NavigationManager navManager
) : ComponentBase
{
    private GroupViewModel Entity { get; set; } = new();
    private List<ListValueResponse> _centers = [];

    private async Task OnSubmit()
    {
        var response = await sender.Send(new CreateGroupCommand(
            Entity.Name,
            Entity.Description,
            Entity.CenterId
        ));

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Group Created Successfully..");
            navManager.NavigateTo($"{Paths.GroupView}/{response.Value}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
    private void OnClear() => Entity = new GroupViewModel();

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName ="New Group";
        await LoadListValues();
    }
    
    private async Task LoadListValues()
    {
        var response = await sender.Send(new GetCentersListValueQuery());
        if (response.IsSuccess)
        {
            _centers = [..response.Value];
        }
    }
    
    private void OnSelectedCenterChanged(Guid id)=> Entity.CenterId = id;
}


