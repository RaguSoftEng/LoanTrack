using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Groups.Commands.Create;
using LoanTrack.Application.Groups.Commands.Update;
using LoanTrack.Application.Groups.Queries.GetGroupById;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Groups;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Groups;

public partial class Edit(
    ISender sender,
    IToastService toastService,
    AppSettingState appSetting
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private GroupViewModel Entity { get; set; } = new();
    private List<ListValueResponse> _centers = [];

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName ="Group";
        await GetGroupAsync();
    }
    
    private async Task GetGroupAsync()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var groupId = Guid.Parse(Id);
            var response = await sender.Send(new GetGroupByIdQuery(groupId));
            if (response.IsSuccess)
            {
                Entity = new GroupViewModel
                {
                    GroupId = response.Value.Id,
                    Name = response.Value.Name,
                    Description = response.Value.Description,
                    Center = response.Value.Center,
                    CenterId = response.Value.CenterId
                };
                await LoadListValues(Entity.CenterId);
                StateHasChanged();
            }
        }
    }
    
    private async Task LoadListValues(Guid centerId)
    {
        var response = await sender.Send(new GetCentersListValueQuery());
        if (response.IsSuccess)
        {
            var centers = response.Value.Select(x => x with { IsSelected = Equals(x.Id, centerId) }).ToList();

            _centers = centers;
        }
    }
    
    private async Task OnSubmit()
    {
        var response = await sender.Send(new UpdateGroupCommand(
            Entity.GroupId,
            Entity.Name,
            Entity.Description,
            Entity.CenterId
        ));

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Group Updated Successfully..");
            NavManager.NavigateTo($"{Paths.GroupView}/{Entity.GroupId}");
            
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
    
    private void OnSelectedCenterChanged(Guid id)=> Entity.CenterId = id;
}

