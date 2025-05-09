using LoanTrack.Application.Centers.Queries.GetById;
using LoanTrack.Web.Shared.Centers;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Centers;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private CenterViewModel Entity { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Group";
        await GetCenterAsync();
    }

    private async Task GetCenterAsync()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var centerId = Guid.Parse(Id);
            var response = await sender.Send(new GetCenterByIdQuery(centerId));
            if (response.IsSuccess)
            {
                Entity = new CenterViewModel
                {
                    Name = response.Value.Name,
                    Description = response.Value.Description ?? string.Empty,
                    Leader = response.Value.CenterLeader
                };
                StateHasChanged();
            }
        }
    }
}

