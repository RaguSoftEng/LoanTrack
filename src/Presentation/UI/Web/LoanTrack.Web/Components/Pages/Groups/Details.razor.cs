using LoanTrack.Application.Groups.Queries.GetGroupById;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Groups;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Groups;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private GroupViewModel Entity { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Group";
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
                    Center = response.Value.Center
                };
                StateHasChanged();
            }
        }
    }
}

