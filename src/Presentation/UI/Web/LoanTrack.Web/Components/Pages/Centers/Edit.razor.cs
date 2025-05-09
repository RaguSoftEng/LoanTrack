using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Commands.Create;
using LoanTrack.Application.Centers.Commands.Update;
using LoanTrack.Application.Centers.Queries.GetById;
using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Application.Customers.Queries.GetCustomer.ByNic;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Centers;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Centers;

public partial class Edit(
    ISender sender,
    IToastService toastService,
    AppSettingState appSetting
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private CenterViewModel Entity { get; set; } = new();
    private string Nic { get; set; } = string.Empty;
    private string SelectedLeader { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName ="Center";
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
                    Id = response.Value.CenterId,
                    Name = response.Value.Name,
                    Description = response.Value.Description ?? string.Empty,
                    Leader = response.Value.CenterLeader,
                    LeaderId = response.Value.CenterLeaderId,
                };
                SelectedLeader = Entity.Leader;
                StateHasChanged();
            }
        }
    }
    
    private async Task FindCustomerByNic()
    {
        if (!string.IsNullOrEmpty(Nic))
        {
            var response = await sender.Send(new GetCustomerByNicQuery(Nic));
            if (response.IsSuccess)
            {
                var leader = response.Value;
                SelectedLeader = $"{leader.FullName}\n{leader.PhoneNumber}\n{leader.Email}\n{leader.Address}";
                Entity.LeaderId = leader.Id;
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }
    private async Task OnSubmit()
    {
        var response = await sender.Send(new UpdateCenterCommand(
            Entity.Id,
            Entity.Name,
            Entity.Description,
            Entity.LeaderId
        ));

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Center Updated Successfully..");
            NavManager.NavigateTo($"{Paths.CenterView}/{Entity.Id}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

