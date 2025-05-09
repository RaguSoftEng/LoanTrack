using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.Customers;

public class CustomerErrors
{
    public static Error NotFound(Guid customerId) =>
        Error.NotFound("Customers.NotFound", $"The customer with the identifier {customerId} was not found");
    
    public static Error NotFound(string nic) =>
        Error.NotFound("Customers.NotFound", $"The customer with the NIC {nic} was not found");  
}
