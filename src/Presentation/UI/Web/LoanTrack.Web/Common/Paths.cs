namespace LoanTrack.Web.Common;

internal static class Paths
{
    #region Employees

    public const string EmployeesIndex = "Employees";
    public const string EmployeeView = "Employees/Details";
    public const string EmployeeNew = "Employees/New";
    public const string EmployeeEdit = "Employees/Edit";

    #endregion
    
    #region ListValues

    public const string ListValuesIndex = "/ListValues/{0}";
    public const string ListValuesIndexPath = "/ListValues/{ListItem}";
    
    #endregion
    #region Customers

    public const string CustomersIndex = "Customers";
    public const string CustomerView = "Customers/Details";
    public const string CustomerNew = "Customers/New";
    public const string CustomerEdit = "Customers/Edit";

    #endregion

    #region Centers

    public const string CentersIndex = "Centers";
    public const string CenterView = "Centers/Details";
    public const string CenterNew = "Centers/New";
    public const string CenterEdit = "Centers/Edit";

    #endregion

    #region Loans

    public const string LoansIndex = "Loans";
    public const string LoanView = "Loans/Details";
    public const string LoanNew = "Loans/New";
    public const string LoanEdit = "Loans/Edit";

    #endregion
    
    #region Installments
    public const string NextInstallmentsIndex = "Installments";
    public const string InstallmentsIndex = "Loan/{0}/Installments";
    public const string InstallmentsIndexPath = "Loan/{LoanId}/Installments";
    public const string InstallmentView = "Loan/{0}/Installment/{1}/Details";
    public const string InstallmentViewPath = "Loan/{LoanId}/Installment/{Id}/Details";
    public const string InstallmentPayment = "Loan/{LoanId}/Installment/{Id}/Payment";
    public const string InstallmentPaymentPath = "Loan/{0}/Installment/{1}/Payment";
    public const string NextInstallmentPayment = "Loan/{LoanId}/Installment/Payment";
    public const string NextInstallmentPaymentPath = "Loan/{0}/Installment/Payment";
    #endregion

    #region Groups

    public const string GroupIndex = "Groups";
    public const string GroupView = "Groups/Details";
    public const string GroupNew = "Groups/New";
    public const string GroupEdit = "Groups/Edit";

    #endregion

    #region Schemes

    public const string SchemesIndex = "Schemes";
    public const string SchemesView = "Schemes/Details";
    public const string SchemesNew = "Schemes/New";
    public const string SchemesEdit = "Schemes/Edit";

    #endregion
    
}
