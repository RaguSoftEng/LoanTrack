using LoanTrack.Application.Customers.Queries.GetCustomer;

namespace LoanTrack.Web.Shared.Customers;

public sealed class CustomerVm
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Nic { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = "Male";
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public Guid GramaNiladhariId { get; set; }
    public string GramaNiladhari { get; set; } = string.Empty;
    public Guid DsDivisionId { get; set; }
    public string DsDivision { get; set; } = string.Empty;
    public Guid DistrictId { get; set; }
    public string District { get; set; } = string.Empty;
    public Guid ProvinceId { get; set; }
    public string Province { get; set; } = string.Empty;
    public Guid CenterId { get; set; }
    public string Center { get; set; } = string.Empty;
    public Guid GroupId { get; set; }
    public string Group { get; set; } = string.Empty;
    public string Occupation { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public string? BankName { get; set; }
    public string? BankBranch { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? AccountName { get; set; }
    public string? WokAddress { get; set; }

    public CustomerVm(){}
    public CustomerVm(CustomerResponse record)
    {
        Id = record.Id;
        Code = record.Code;
        Nic = record.Nic;
        FullName = record.FullName;
        Gender = record.Gender;
        Email = record.Email;
        PhoneNumber = record.PhoneNumber;
        Address = record.Address;
        GramaNiladhariId = record.GramaNiladhariId;
        GramaNiladhari = record.GramaNiladhari;
        DsDivisionId = record.DsDivisionId;
        DsDivision = record.DsDivision;
        DistrictId = record.DistrictId;
        District = record.District;
        ProvinceId = record.ProvinceId;
        Province = record.Province;
        CenterId = record.CenterId;
        Center = record.Center;
        GroupId = record.GroupId ?? Guid.Empty;
        Group = record.Group;
        Occupation = record.Occupation;
        DateOfBirth = record.DateOfBirth;
        BankName = record.BankName;
        BankAccountNumber = record.BankAccountNumber;
        AccountName = record.AccountName;
        BankBranch = record.Branch;
        WokAddress = record.WorkAddress;
    }
}
