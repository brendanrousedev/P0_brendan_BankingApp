using Microsoft.EntityFrameworkCore;
using P0_brendan_BankingApp.POCO;

public class CustomerDao 
{
    P0BrendanBankingDbContext Context;
    BasicConsole io = new IOConsole();
    public CustomerDao(P0BrendanBankingDbContext context)
    {
        Context = context;
    }
    public Customer? GetCustomerByUsername(string customerUsername)
    {
        return Context.Customers.SingleOrDefault(c => c.CustomerUsername == customerUsername);
    }

    public Customer GetCustomerById(int id)
    {
        return Context.Customers.Find(id);
    }

    public int CreateNewCustomer(Customer customer)
    {
        Context.Customers.Add(customer);
        Context.SaveChanges();
        return customer.CustomerId;
    }

    public bool CustomerExists(string username)
    {
        return Context.Customers.Any(c => c.CustomerUsername == username);
    }

    public void DeleteCustomerByUsername(string username)
    {
        var customer = GetCustomerByUsername(username);
        if (customer != null)
        {
            Context.Customers.Remove(customer);
            Context.SaveChanges();
        }
        else
        {
            io.DisplayDoesNotExist(username);
        }

    }

}