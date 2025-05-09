using Blazored.Toast.Services;
using LoanTrack.Application.Centers.Queries.GetAsListValue;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.Customers.Commands.UpdateCustomer;
using LoanTrack.Application.Customers.Queries.GetCustomer.ById;
using LoanTrack.Application.Groups.Queries.GetGroupsListValues;
using LoanTrack.Application.ListValues.Queries.GetListValuesByType;
using LoanTrack.Domain.ListValues;
using LoanTrack.Web.Common;
using LoanTrack.Web.Shared.Common;
using LoanTrack.Web.Shared.Customers;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace LoanTrack.Web.Components.Pages.Customers;

public partial class Edit(
    ISender sender,
    AppSettingState appSetting,
    IToastService toastService,
    NavigationManager navigationManager
) : ComponentBase
{
    [Parameter] public string Id { get; set; }
    private CustomerVm Entity { get; set; } = new();
    private List<ListValueResponse> _provinces = [];
    private List<ListValueResponse> _districts = [];
    private List<ListValueResponse> _dsDivision = [];
    private List<ListValueResponse> _gramaNiladharies = [];
    private List<ListValueResponse> _centers = [];
    private List<ListValueResponse> _groups = [];
    private bool _dataIsLoaded;

    protected override async Task OnInitializedAsync()
    {
        appSetting.CurrentPageName = "Customer";
        if (!_dataIsLoaded)
        {
            await GetCustomer();
            await LoadListvalues();
            _dataIsLoaded = true;
            StateHasChanged();
        }
    }

    private async Task GetCustomer()
    {
        var customerId = Guid.Parse(Id);
        var response = await sender.Send(new GetCustomerByIdQuery(customerId));
        if (response.IsSuccess)
        {
            Entity = new CustomerVm(response.Value);
        }
    }

    private async Task LoadListvalues()
    {
        var response = await sender.Send(new GetListValuesByTypeQuery(ListTypes.PROVINCES, Guid.Empty));
        if (response.IsSuccess)
        {
            var provinces = response.Value
                .Select(x => x with { IsSelected = Equals(x.Id, Entity.ProvinceId) })
                .ToList();
            
            if (!provinces.Any(p => p.IsSelected))
            {
                provinces.Add(new ListValueResponse(Guid.Empty, "--- Select ---", true));
            }

            _provinces = provinces;
        }

        if (Entity.ProvinceId != Guid.Empty)
        {
            await OnSelectedProvinceChanged(Entity.ProvinceId);
        }

        if (Entity.DistrictId != Guid.Empty)
        {
            await OnSelectedDistrictChanged(Entity.DistrictId);
        }

        if (Entity.DsDivisionId != Guid.Empty)
        {
            await OnSelectedDsDivisionChanged(Entity.DsDivisionId);
        }

        var centersResponse = await sender.Send(new GetCentersListValueQuery());
        if (centersResponse.IsSuccess)
        {
            var result = centersResponse.Value
                .Select(x => x with { IsSelected = Equals(x.Id, Entity.CenterId) })
                .ToList();

            if (!result.Any(p => p.IsSelected))
            {
                result.Add(new ListValueResponse(Guid.Empty, "--- Select ---", true));
            }

            _centers = result;
        }

        if (Entity.CenterId != Guid.Empty)
        {
            await OnSelectedCenterChanged(Entity.CenterId);
        }
    }

    private async Task LoadGroups(Guid centerId)
    {
        _groups = [];
        var response = await sender.Send(new GetGroupsListValueQuery(centerId));
        if (response.IsSuccess)
        {
            var result = response.Value
                .Select(x => x with { IsSelected = Equals(x.Id, Entity.GroupId) })
                .ToList();

            if (!result.Any(p => p.IsSelected))
            {
                result.Add(new ListValueResponse(Guid.Empty, "--- Select ---", true));
            }

            _groups = result;
        }
    }

    private async Task OnSelectedProvinceChanged(Guid id)
    {
        Entity.ProvinceId = id;
        _districts = [];
        if (Entity.ProvinceId == Guid.Empty) return;
        var response = await sender.Send(new GetListValuesByTypeQuery(ListTypes.DISTRICTS, id));
        if (response.IsSuccess)
        {
            var districts = response.Value
                .Select(x => x with { IsSelected = Equals(x.Id, Entity.DistrictId) })
                .ToList();

            if (!districts.Any(p => p.IsSelected))
            {
                districts.Add(new ListValueResponse(Guid.Empty, "--- Select ---", true));
            }

            _districts = districts;
        }
    }

    private async Task OnSelectedDistrictChanged(Guid id)
    {
        Entity.DistrictId = id;
        _dsDivision = [];
        var response = await sender.Send(new GetListValuesByTypeQuery(ListTypes.DsDivisions, id));
        if (response.IsSuccess)
        {
            var divisions = response.Value
                .Select(x => x with { IsSelected = Equals(x.Id, Entity.DsDivisionId) })
                .ToList();

            if (!divisions.Any(p => p.IsSelected))
            {
                divisions.Add(new ListValueResponse(Guid.Empty, "--- Select ---", true));
            }

            _dsDivision = divisions;
        }
    }

    private async Task OnSelectedDsDivisionChanged(Guid id)
    {
        Entity.DsDivisionId = id;
        _gramaNiladharies = [];
        var divResponse = await sender.Send(new GetListValuesByTypeQuery(ListTypes.GRAMANILADHARI, id));
        if (divResponse.IsSuccess)
        {
            var results = divResponse.Value
                .Select(x => x with { IsSelected = Equals(x.Id, Entity.GramaNiladhariId) })
                .ToList();

            if (!results.Any(p => p.IsSelected))
            {
                results.Add(new ListValueResponse(Guid.Empty, "--- Select ---", true));
            }

            _gramaNiladharies = results;
        }
    }

    private void OnSelectedGramaniladhariChanged(Guid id) => Entity.GramaNiladhariId = id;

    private async Task OnSelectedCenterChanged(Guid id)
    {
        Entity.CenterId = id;
        await LoadGroups(id);
    }

    private void OnSelectedGroupChanged(Guid id) => Entity.GroupId = id;

    private void OnClear() => Entity = new CustomerVm();

    private async Task OnSubmit()
    {
        var response = await sender.Send(
            new UpdateCustomerCommand(
                Entity.Id,
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
            toastService.ShowSuccess("Customer updated Successfully...");
            navigationManager.NavigateTo($"{Paths.CustomerView}/{Entity.Id}");
        }
        else
        {
            toastService.ShowError(response.Error.Description);
        }
    }
}

