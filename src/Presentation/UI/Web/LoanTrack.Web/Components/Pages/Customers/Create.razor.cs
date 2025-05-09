using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Commands.CreateCustomer;
using LoanTrack.Application.Groups.Queries.GetGroupsListValues;
using LoanTrack.Application.ListValues.Queries.GetListValuesByType;
using LoanTrack.Domain.ListValues;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Customers;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Customers;

public partial class Create(
    ISender sender,
    AppSettingState appSetting,
    IToastService toastService,
    NavigationManager navigationManager
) : ComponentBase
{
    private CustomerVm Entity { get; set; } = new();
    private List<ListValueResponse> _provinces = [];
    private List<ListValueResponse> _districts = [];
    private List<ListValueResponse> _dsDivision = [];
    private List<ListValueResponse> _gramaNiladharies = [];
    private List<ListValueResponse> _centers = [];
    private List<ListValueResponse> _groups = [];
    
    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "New Customer";
        await LoadListvalues();
    }

    private async Task LoadListvalues()
    {
        var response = await sender.Send(new GetListValuesByTypeQuery(ListTypes.PROVINCES, Guid.Empty));
        if (response.IsSuccess)
        {
            _provinces = [.. response.Value];
        }

        var centersResponse = await sender.Send(new GetCentersListValueQuery());
        if (centersResponse.IsSuccess)
        {
            _centers = [.. centersResponse.Value];
        }

    }

    private async Task LoadGroups(Guid centerId)
    {
        _groups = [];
        var response =  await sender.Send(new GetGroupsListValueQuery(centerId));
        if (response.IsSuccess)
        {
            _groups = [.. response.Value];
        }
    }

    private async Task OnSelectedProvinceChanged(Guid id)
    {
        Entity.ProvinceId = id;
        var response = await sender.Send(new GetListValuesByTypeQuery(ListTypes.DISTRICTS, id));
        if (response.IsSuccess)
        {
            _districts = [.. response.Value];
        }
    }
    private async Task OnSelectedDistrictChanged(Guid id)
    {
        Entity.DistrictId = id;
        var response = await sender.Send(new GetListValuesByTypeQuery(ListTypes.DsDivisions, id));
        if (response.IsSuccess)
        {
            _dsDivision = [.. response.Value];
        }
    }

    private async Task OnSelectedDsDivisionChanged(Guid id)
    {
        Entity.DsDivisionId = id;
        var divResponse = await sender.Send(new GetListValuesByTypeQuery(ListTypes.GRAMANILADHARI, id));
        if (divResponse.IsSuccess)
        {
            _gramaNiladharies = [.. divResponse.Value];
        }
    }
    private void OnSelectedGramaniladhariChanged(Guid id) => Entity.GramaNiladhariId = id;

    private async Task OnSelectedCenterChanged(Guid id)
    {
        Entity.CenterId = id;
        await LoadGroups(id);
    }
    private void OnSelectedGroupChanged(Guid id) => Entity.GroupId = id;
    
    private void OnClear () => Entity = new CustomerVm();

    private async Task OnSubmit()
    {
        var response = await sender.Send(
            new CreateCustomerCommand(
                Entity.Nic,
                Entity.FullName,
                Entity.Gender,
                Entity.Email,
                Entity.PhoneNumber,
                Entity.Address,
                Entity.DsDivisionId,
                Entity.DistrictId,
                Entity.ProvinceId,
                Entity.GramaNiladhariId,
                Entity.CenterId,
                Entity.GroupId,
                Entity.Occupation,
                Entity.DateOfBirth,
                Entity.BankName,
                Entity.BankBranch,
                Entity.BankAccountNumber,
                Entity.AccountName,
                Entity.WokAddress
            )
        );

        if (response.IsSuccess)
        {
            toastService.ShowSuccess("Customer Created Successfully...");
            navigationManager.NavigateTo($"{Paths.CustomerView}/{response.Value}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

