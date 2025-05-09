using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Commands.Create;
using LoanTrack.Application.Customers.Queries.GetCustomer;
using LoanTrack.Application.Customers.Queries.GetCustomer.ByNic;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Centers;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Centers;

public partial class Create(
    ISender sender,
    IToastService toastService,
    AppSettingState appSetting,
    NavigationManager navManager
) : ComponentBase
{
    private CenterViewModel Entity { get; set; } = new();
    private string Nic { get; set; } = string.Empty;
    private CustomerResponse Leader { get; set; }
    private string SelectedLeader { get; set; } = string.Empty;
    
    private void OnClear() => Entity = new CenterViewModel();

    protected override void OnInitialized()
    {
        appSetting.CurrentPageName ="New Center";
    }
    
    private async Task FindCustomerByNic()
    {
        if (!string.IsNullOrEmpty(Nic))
        {
            var response = await sender.Send(new GetCustomerByNicQuery(Nic));
            if (response.IsSuccess)
            {
                Leader = response.Value;
                SelectedLeader = $"{Leader.FullName}\n{Leader.PhoneNumber}\n{Leader.Email}\n{Leader.Address}";
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }
    private async Task OnSubmit()
    {
        var response = await sender.Send(new CreateCenterCommand(
            Entity.Name,
            Entity.Description,
            Entity.LeaderId
        ));

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Center Created Successfully..");
            navManager.NavigateTo($"{Paths.CenterView}/{response.Value}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

