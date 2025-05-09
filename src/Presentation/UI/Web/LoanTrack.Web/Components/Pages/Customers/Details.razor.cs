using LoanTrack.Application.Customers.Queries.GetCustomer.ById;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Customers;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Customers;

public partial class Details(
    ISender sender,
    AppSettingState appSettingState
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private CustomerVm Entity { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        appSettingState.CurrentPageName = "Customer";
        await LoadCustomer();
    }

    private async Task LoadCustomer()
    {
        if (!string.IsNullOrEmpty(Id))
        {
            var customerId = Guid.Parse(Id);
            var response = await sender.Send(new GetCustomerByIdQuery(customerId));
            if (response.IsSuccess)
            {
                Entity = new CustomerVm(response.Value);
                StateHasChanged();
            }
        }
    }
}

