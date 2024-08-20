using Microsoft.EntityFrameworkCore;
using P0_brendan_BankingApp.POCO;

public class CustomerDao 
{
    P0BrendanBankingDbContext DBContext;
    public CustomerDao(P0BrendanBankingDbContext context)
    {
        DBContext = context;
    }
    public Customer? GetCustomerByUsername(string customerUsername)
    {
        return DBContext.Customers.SingleOrDefault(c => c.CustomerUsername == customerUsername);
    }
}