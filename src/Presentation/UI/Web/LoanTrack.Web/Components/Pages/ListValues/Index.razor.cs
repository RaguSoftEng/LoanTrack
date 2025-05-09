using Blazored.Toast.Services;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.ListValues.Commands.BulkUpload;
using LoanTrack.Application.ListValues.Commands.CreateListValueOption;
using LoanTrack.Application.ListValues.Commands.Update;
using LoanTrack.Application.ListValues.Queries.GetListValuesByType;
using LoanTrack.Domain.ListValues;
using LoanTrack.Web.Shared.Common;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LoanTrack.Web.Components.Pages.ListValues;

public partial class Index(
    ISender sender,
    AppSettingState appSetting,
    IToastService toastService
) : ComponentBase
{
    [Parameter]
    public string ListItem { get; set; } = string.Empty;
    private List<ListValuesVm> _listValues = [];
    private string _selectedListType = ListTypes.PROVINCES;
    private string _parentListType = "";
    private List<ListValueResponse> _parentListValues = [];
    private Guid _parentId = Guid.Empty;
    private string ListValue { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _selectedListType = string.IsNullOrEmpty(ListItem) ? ListTypes.PROVINCES : ListItem;
        appSetting.CurrentPageName = _selectedListType;
        await LoadParentListValues(_selectedListType);
        await GetListValuesAsync(_selectedListType, _parentId);
    }

    private async Task LoadParentListValues(string listType)
    {
        _parentListValues = [];
        _parentListType = listType switch
        {
            ListTypes.DISTRICTS => ListTypes.PROVINCES,
            ListTypes.DsDivisions => ListTypes.DISTRICTS,
            ListTypes.GRAMANILADHARI => ListTypes.DsDivisions,
            _ => ""
        };

        if (!string.IsNullOrEmpty(_parentListType))
        {
            var response = await sender.Send(new GetListValuesByTypeQuery(_parentListType, Guid.Empty));
            if (response.IsSuccess)
            {
                _parentListValues = [..response.Value];
                if (_parentListValues.Any())
                {
                    _parentId = Guid.Parse(_parentListValues[0].Id.ToString() ?? string.Empty);
                }
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }

    private async Task OnParentChanged(Guid parentId)
    {
        _parentId = parentId;
        await GetListValuesAsync(_selectedListType, _parentId);
    }

    private async Task GetListValuesAsync(string listType, Guid parentId)
    {
        ListValue = string.Empty;
        _listValues = [];
        var response = await sender.Send(new GetListValuesByTypeQuery(listType, parentId));
        if (response.IsSuccess)
        {
            _listValues = [.. response.Value.Select(ListValuesVm.LoadValues)];
        }
    }

    private async Task Add()
    {
        if (!string.IsNullOrEmpty(ListValue))
        {
            var response =  await sender.Send(new CreateListValueCommand(_selectedListType, ListValue, _parentId));
            if (response.IsSuccess)
            {
                _listValues.Add(new ListValuesVm
                {
                   Id = response.Value,
                   Value = ListValue
                });
                toastService.ShowSuccess("Value added successfully...");
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
            ListValue = string.Empty;
        }
    }
    
    private async Task Upload(InputFileChangeEventArgs e)
    {
        var file = e.File;

        var extension = Path.GetExtension(file.Name).ToLower(System.Globalization.CultureInfo.CurrentCulture);
        if (extension != ".csv")
        {
            toastService.ShowError("Only .csv files are supported.");
            return;
        }

        await using var stream = file.OpenReadStream(10_000_000); // 10MB limit
        var values = await ReadCsvFile(stream);

        if (values.Any())
        {
            var response = await sender.Send(new ListValuesBulkUploadCommand(_selectedListType, _parentId, values));
            if (response.IsSuccess)
            {
                toastService.ShowSuccess("Values added successfully...");
            }
            else
            {
                toastService.ShowError(response.Error.Description);
            }
        }
    }

    private static async Task<List<string>> ReadCsvFile(Stream stream)
    {
        using var reader = new StreamReader(stream);
        List<string> dataList = [];

        try
        {
            while (await reader.ReadLineAsync() is { } line)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    dataList.Add(line.Trim());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }

        return dataList;
    }

    private void EditClicked(Guid id)
    {
        _listValues.ForEach(x => x.IsEdit = x.Id == id);
    }

    private async Task OnSaveClicked(Guid id)
    {
        var item = _listValues.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            item.IsEdit = false;
           var response = await sender.Send(new ListValueUpdateCommand(id, item.Value));
           if (response.IsSuccess)
           {
               toastService.ShowSuccess("Value updated successfully...");
           }
           else
           {
               toastService.ShowError(response.Error.Description);
           }
        }
    }
}

internal class ListValuesVm
{
   public Guid Id { get; set; }
   public string Value { get; set; } = string.Empty;
   public bool IsEdit { get; set; }

    public ListValuesVm()
    {
        
    }

    public static ListValuesVm LoadValues(ListValueResponse response)
        => new()
        {
            Id = Guid.Parse(response.Id.ToString() ?? string.Empty),
            Value = response.Value
        };
}

