using LoanTrack.Domain.Common;
using LoanTrack.Domain.Common.Constants;
using LoanTrack.Domain.Customers;
using LoanTrack.Domain.LoanSchemes;

namespace LoanTrack.Domain.Loans;

public sealed class Loan : AuditableEntity
{
    public string LoanNumber { get; private set; }
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; }
    public Guid? LoanSchemeId { get; private set; }
    public LoanScheme? LoanScheme { get; private set; }
    public string LoanOfficer { get; private set; }
    public double LoanAmount { get; private set; }
    public string InterestType { get; private set; }
    public double InterestRate { get; private set; }
    public string InstallmentType { get; private set; }
    public int DurationInInterestUnits { get; private set; }
    public int RepaymentDurations { get; private set; }
    public double InstallmentAmount { get; private set; }
    public DateOnly? IssuanceDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public DateOnly? NextInstallmentDate { get; private set; }
    public string LoanDisbursementMethod { get; private set; }
    public string LoanRepaymentMethod { get; private set; }
    public string GuarantorsInformation { get; private set; }
    public string LoanStatus { get; private set; }
    public DateOnly? ClosedDate { get; private set; }
    public double TotalAmountPayable { get; private set; }
    public double PaidAmount { get; private set; }
    public double ProcessingFee { get; private set; }
    public double InsuranceAmount { get; private set; }
    

    private Loan() { }
    
    
    public static Loan Create(
        string loanNumber,
        Guid customerId,
        Guid? loanSchemeId,
        string loanOfficer,
        string interestType,
        double loanAmount,
        double interestRate,
        string installmentType,
        int durationInInterestUnits,
        int repaymentDurations,
        double installmentAmount,
        DateOnly? issuanceDate,
        DateOnly? nextInstallmentDate,
        string loanDisbursementMethod,
        string loanRepaymentMethod,
        string guarantorsInformation,
        double totalAmountPayable,
        double processingFee,
        double insuranceAmount
    )
    {
        var loan = new Loan
        {
            LoanNumber = loanNumber,
            CustomerId = customerId,
            LoanSchemeId = loanSchemeId == Guid.Empty ? null : loanSchemeId,
            LoanOfficer = loanOfficer,
            InterestType = InterestTypes.Validate(interestType),
            DurationInInterestUnits = durationInInterestUnits,
            LoanAmount = loanAmount,
            InterestRate = interestRate,
            InstallmentType = installmentType,
            RepaymentDurations = repaymentDurations,
            InstallmentAmount = installmentAmount,
            IssuanceDate = issuanceDate,
            NextInstallmentDate = nextInstallmentDate,
            LoanDisbursementMethod = LoanDisbursementMethods.Validate(loanDisbursementMethod),
            LoanRepaymentMethod = LoanRepaymentMethods.Validate(loanRepaymentMethod),
            GuarantorsInformation = guarantorsInformation,
            LoanStatus = LoanStatuses.Pending,
            TotalAmountPayable = totalAmountPayable,
            ProcessingFee = processingFee,
            InsuranceAmount = insuranceAmount
        };
        
        return loan;
    }

    public void Update(
        Guid? loanSchemeId,
        string loanOfficer,
        double loanAmount,
        string interestType,
        double interestRate,
        string installmentType,
        int durationInInterestUnits,
        int repaymentDurations,
        double installmentAmount,
        DateOnly? issuanceDate,
        DateOnly? nextInstallmentDate,
        string loanDisbursementMethod,
        string loanRepaymentMethod,
        string guarantorsInformation,
        double totalAmountPayable,
        double processingFee,
        double insuranceAmount
    )
    {
        LoanSchemeId = loanSchemeId == Guid.Empty ? null : loanSchemeId;
        LoanOfficer = loanOfficer;
        LoanAmount = loanAmount;
        InterestType = interestType;
        InterestRate = interestRate;
        InstallmentType = installmentType;
        DurationInInterestUnits = durationInInterestUnits;
        RepaymentDurations = repaymentDurations;
        InstallmentAmount = installmentAmount;
        IssuanceDate = issuanceDate;
        NextInstallmentDate = nextInstallmentDate;
        LoanDisbursementMethod = loanDisbursementMethod;
        LoanRepaymentMethod = loanRepaymentMethod;
        GuarantorsInformation = guarantorsInformation;
        TotalAmountPayable = totalAmountPayable;
        ProcessingFee = processingFee;
        InsuranceAmount = insuranceAmount;
    }

    public void Approve() => LoanStatus = LoanStatuses.Approved;
    public void Reject() => LoanStatus = LoanStatuses.Rejected;
    public void CanceledByCustomer() => LoanStatus = LoanStatuses.CanceledByCustomer;

    public void Issue(DateOnly? issuanceDate, DateOnly firstInstallmentDate)
    {
        LoanStatus = LoanStatuses.Ongoing;
        IssuanceDate = issuanceDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        NextInstallmentDate = firstInstallmentDate;
        SetEndDate();
    }

    public void Close(DateOnly? closeDate)
    {
        LoanStatus = LoanStatuses.Closed;
        ClosedDate = closeDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public LoanInstallment GenerateInstallments(int index)
    {
        if (NextInstallmentDate == null
            || !(NextInstallmentDate.Value > DateOnly.MinValue && NextInstallmentDate.Value < DateOnly.MaxValue)
            || NextInstallmentDate < IssuanceDate
        )
            throw new InvalidOperationException("Valid Next Installment date must be provided.");
        
        NextInstallmentDate = index == 1 ? NextInstallmentDate : GetNextInstallmentDate(NextInstallmentDate.Value);
        
        var balance = TotalAmountPayable - PaidAmount;
        var payable = balance > InstallmentAmount ? InstallmentAmount : balance;

        var installment = LoanInstallment.Create(Id, index, NextInstallmentDate.Value, payable);
        
        return installment;
    }

    private DateOnly GetNextInstallmentDate(DateOnly baseDate)
    {
        return InstallmentType switch
        {
            InstallmentTypes.Daily => baseDate.AddDays(1),
            InstallmentTypes.Weekly => baseDate.AddDays(7),
            InstallmentTypes.Monthly => baseDate.AddMonths(1),
            InstallmentTypes.Yearly => baseDate.AddYears(1),
            _ => baseDate.AddDays(1)
        };
    }

    private void SetEndDate()
    {
        EndDate = InstallmentType switch
        {
            InstallmentTypes.Daily => NextInstallmentDate!.Value.AddDays(RepaymentDurations),
            InstallmentTypes.Weekly => NextInstallmentDate!.Value.AddDays(RepaymentDurations*7),
            InstallmentTypes.Monthly => NextInstallmentDate!.Value.AddMonths(RepaymentDurations),
            InstallmentTypes.Yearly => NextInstallmentDate!.Value.AddYears(RepaymentDurations),
            _ =>  NextInstallmentDate!.Value.AddDays(RepaymentDurations)
        };
    }

    public void InstallmentPaid(double paidAmount)
    {
        PaidAmount += paidAmount;
        if (TotalAmountPayable > PaidAmount) return;

        LoanStatus = LoanStatuses.Closed;
        ClosedDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }

}
