using LoanTrack.Domain.Centers;
using LoanTrack.Domain.Common;
using LoanTrack.Domain.Groups;
using LoanTrack.Domain.ListValues;

namespace LoanTrack.Domain.Customers;

public sealed class Customer : AuditableEntity
{
    public int Code { get; private set; }
    public string Nic { get; private set; }
    public string FullName { get; private set; }
    public string Gender { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Address { get; private set; }
    public Guid? GramaNiladhariId { get; private set; }
    public ListValue? GramaNiladhari { get; private set; }
    public Guid? DsDivisionId { get; private set; }
    public ListValue? DsDivision { get; private set; } = null!;
    public Guid? DistrictId { get; private set; }
    public ListValue? District { get; private set; }
    
    public Guid? ProvinceId { get; private set; }
    public ListValue? Province { get; private set; }
    public Guid? CenterId { get; private set; }
    public Center? Center { get; private set; } = null!;
    public Guid? GroupId { get; private set; }
    public CustomerGroup? Group { get; private set; } = null!;
    public string? Occupation { get; private set; }
    public string? WorkAddress { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string? BankName { get; private set; }
    public string? BankBranch { get; private set; }
    public string? BankAccountNumber { get; private set; }
    public string? AccountName { get; private set; }

    private Customer() { }

    public static Customer Create(
        string nic,
        string fullName,
        string gender,
        string email,
        string phoneNumber,
        string address,
        Guid gramaNiladhariId,
        Guid dsDivisionId,
        Guid districtId,
        Guid provinceId,
        Guid centerId,
        Guid? groupId,
        string? occupation,
        DateOnly dateOfBirth,
        string? bankName,
        string? bankBranch,
        string? bankAccountNumber,
        string? accountName,
        string? workAddress
    ) => new()
    {
        Nic = nic,
        FullName = fullName,
        Gender = gender,
        Email = email,
        PhoneNumber = phoneNumber,
        Address = address,
        GramaNiladhariId = gramaNiladhariId == Guid.Empty ? null : gramaNiladhariId,
        DsDivisionId = dsDivisionId == Guid.Empty ? null : dsDivisionId,
        DistrictId = districtId == Guid.Empty ? null : districtId,
        ProvinceId = provinceId == Guid.Empty ? null : provinceId,
        CenterId = centerId == Guid.Empty ? null : centerId,
        GroupId = groupId == Guid.Empty ? null : groupId,
        Occupation = occupation,
        DateOfBirth = dateOfBirth,
        BankName = bankName,
        BankBranch = bankBranch,
        BankAccountNumber = bankAccountNumber,
        AccountName = accountName,
        WorkAddress = workAddress
    };
    
    public void Update(
        string nic,
        string fullName,
        string gender,
        string email,
        string phoneNumber,
        string address,
        Guid? gramaNiladhariId,
        Guid? dsDivisionId,
        Guid? districtId,
        Guid? provinceId,
        Guid? centerId,
        Guid? groupId,
        string? occupation,
        string? workAddress,
        DateOnly dateOfBirth,
        string? bankName,
        string? bankBranch,
        string? bankAccountNumber,
        string? accountName
    )
    {
        if (Nic != nic) Nic = nic;
        if (FullName != fullName) FullName = fullName;
        if (Gender != gender) Gender = gender;
        if (Email != email) Email = email;
        if (PhoneNumber != phoneNumber) PhoneNumber = phoneNumber;
        if (Address != address) Address = address;
        if (GramaNiladhariId != gramaNiladhariId) GramaNiladhariId = gramaNiladhariId;
        if (DsDivisionId != dsDivisionId) DsDivisionId = dsDivisionId;
        if (DistrictId != districtId) DistrictId = districtId;
        if (ProvinceId != provinceId) ProvinceId = provinceId;
        if (CenterId != centerId) CenterId = centerId;
        if (GroupId != groupId) GroupId = groupId;
        if (Occupation != occupation) Occupation = occupation;
        if (WorkAddress != workAddress) WorkAddress = workAddress;
        if (DateOfBirth != dateOfBirth) DateOfBirth = dateOfBirth;
        if (BankName != bankName) BankName = bankName;
        if (BankBranch != bankBranch) BankBranch = bankBranch;
        if (BankAccountNumber != bankAccountNumber) BankAccountNumber = bankAccountNumber;
        if (AccountName != accountName) AccountName = accountName;
    }
}
